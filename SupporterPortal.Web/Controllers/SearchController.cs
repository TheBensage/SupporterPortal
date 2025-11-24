using Microsoft.AspNetCore.Mvc;
using SupporterPortal.Application.Models.SiteSearch;
using SupporterPortal.Application.Services;

[Route("api/search")]
[ApiController]
public class SiteSearchController : Controller
{
    private readonly ISiteSearchService _siteSearchService;

    public SiteSearchController(ISiteSearchService siteSearchService)
    {
        _siteSearchService = siteSearchService;
    }

    [HttpGet]
    public ActionResult<SiteSearchResponse> Search(
        [FromQuery] string[]? contentTypes,
        [FromQuery] string searchTerm = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 6,
        [FromQuery] string? orderBy = null,
        [FromQuery] int? maxSize = null)
    {
        SiteSearchRequest request = new()
        {
            SearchTerm = searchTerm,
            ContentTypeAliases = contentTypes?.ToList() ?? new List<string>(),
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            MaxSize = maxSize.HasValue && maxSize > 0
                ? maxSize.Value
                : null
        };

        SiteSearchResponse result = _siteSearchService.GetPages(request);
        return Ok(result);
    }
}
