using Examine;
using Examine.Search;
using SupporterPortal.Application.Models.SiteSearch;
using SupporterPortal.Application.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;

namespace SupporterPortal.Web.Services;

public class ExamineSiteSearchService : ISiteSearchService
{
    private readonly IExamineManager _examineManager;
    private readonly IUmbracoContextAccessor _ctx;

    public ExamineSiteSearchService(IExamineManager examineManager, IUmbracoContextAccessor ctx)
    {
        _examineManager = examineManager;
        _ctx = ctx;
    }

    public SiteSearchResponse GetPages(SiteSearchRequest request)
    {
        string[] excludeContentTypes = ["siteSettings", "homePage"];

        IEnumerable<(string Id, float Score)> allMatches = [];

        if (_examineManager.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out IIndex? index))
        {
            IQuery query = index.Searcher.CreateQuery(IndexTypes.Content);

            string[] contentTypes = request.ContentTypeAliases?
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .ToArray()
                                    ?? Array.Empty<string>();

            IBooleanOperation boolQuery = contentTypes.Length switch
            {
                0 => query.All() as IBooleanOperation ?? throw new InvalidOperationException("Query must be boolean"),
                1 => query.NodeTypeAlias(contentTypes[0]),
                _ => query.GroupedOr(["__NodeTypeAlias"], contentTypes)
            };

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string term = $"*{request.SearchTerm}*";
                boolQuery = boolQuery.And().ManagedQuery(term, ["searchTitle", "searchSummary", "nodeName"]);
            }

            boolQuery = boolQuery.Not().GroupedOr(["__NodeTypeAlias"], excludeContentTypes);

            allMatches = boolQuery.Execute()?
                                .Select(x => (x.Id, x.Score))
                                .ToArray()
                                ?? Array.Empty<(string, float)>();

            if (request.MaxSize.HasValue && request.MaxSize.Value > 0)
            {
                allMatches = allMatches.Take(request.MaxSize.Value);
            }
        }

        int totalCount = allMatches.Count();

        (string Id, float Score)[] pageMatches = allMatches
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArray();

        IUmbracoContext umbracoContext = _ctx.GetRequiredUmbracoContext();
        List<SiteSearchResult> items = new();

        string? fallbackImageUrl = null;
        var root = umbracoContext.Content?.GetAtRoot().FirstOrDefault();
        if (root != null)
        {
            IPublishedContent? siteSettings = root.DescendantsOrSelf("siteSettings").FirstOrDefault();
            fallbackImageUrl = siteSettings?.Value<IPublishedContent?>("siteImage")?.Url();
        }

        foreach ((string id, float score) in pageMatches)
        {
            if (!int.TryParse(id, out int contentId)) continue;

            IPublishedContent? content = umbracoContext.Content?.GetById(contentId);
            if (content == null) continue;

            items.Add(new SiteSearchResult
            {
                Id = id,
                Title = GetFallbackString(content, "searchTitle", content.Name),
                Summary = content.Value<string?>("searchSummary") ?? "",
                Url = content.Url(),
                ContentType = content.ContentType.Alias,
                Date = MapContentDate(content),
                ImageUrl = content.Value<IPublishedContent?>("searchImage")?.Url() ?? fallbackImageUrl,
                Score = score
            });
        }

        List<SiteSearchResult> orderedItems = request.OrderBy?.ToLower() switch
        {
            "date" => items.OrderByDescending(x => x.Date).ToList(),
            "alphabetical" => items.OrderBy(x => x.Title).ToList(),
            "score" => items.OrderByDescending(x => x.Score).ToList(),
            _ => items.OrderByDescending(x => x.Score).ToList()
        };

        return new SiteSearchResponse
        {
            Results = orderedItems,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    private static string GetFallbackString(IPublishedContent content, string alias, string fallback)
    {
        var value = content.Value<string?>(alias);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }


    private static DateTime MapContentDate(IPublishedContent content)
    {
        return content.UpdateDate != default ? content.UpdateDate : content.CreateDate;
    }
}
