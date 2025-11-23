using Examine;
using Examine.Search;
using SupporterPortal.Application.Models.SiteSearch;
using SupporterPortal.Application.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;

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
        IEnumerable<string>? ids = Array.Empty<string>();

        if (_examineManager.TryGetIndex(Constants.UmbracoIndexes.ExternalIndexName, out IIndex? index))
        {
            IQuery query = index.Searcher.CreateQuery(IndexTypes.Content);

            IBooleanOperation? boolQuery = (IBooleanOperation)(request.ContentTypeAliases != null && request.ContentTypeAliases.Any()
                ? request.ContentTypeAliases.Count() == 1
                    ? query.NodeTypeAlias(request.ContentTypeAliases.First())
                    : query.GroupedOr(["__NodeTypeAlias"], request.ContentTypeAliases.ToArray())
                : query.All());

            boolQuery ??= query.All() as IBooleanOperation;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string term = $"*{request.SearchTerm}*";

                boolQuery = boolQuery?.And()
                                      .ManagedQuery(term, ["searchTitle", "searchSummary", "nodeName"]);
            }

            ids = boolQuery?.Execute()?.Select(x => x.Id);

        }

        IUmbracoContext umbracoContext = _ctx.GetRequiredUmbracoContext();
        List<SiteSearchResult> items = new List<SiteSearchResult>();

        if (ids != null)
        {
            foreach (string id in ids)
            {
                if (!int.TryParse(id, out int contentId)) continue;

                IPublishedContent? content = umbracoContext?.Content?.GetById(contentId);
                if (content == null) continue;

                items.Add(new SiteSearchResult
                {
                    Id = id,
                    Title = content.Value<string>("searchTitle") ?? content.Name,
                    Summary = content.Value<string>("searchSummary") ?? "",
                    Url = content.Url(),
                    ContentType = content.ContentType.Alias,
                    ImageUrl = content.Value<IPublishedContent>("searchImage")?.Url()
                });
            }
        }

        int total = items.Count;

        List<SiteSearchResult> pageItems = items
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new SiteSearchResponse
        {
            Results = pageItems,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }


}
