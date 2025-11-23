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
        [FromQuery] int pageSize = 6)
    {
        SiteSearchRequest request = new()
        {
            SearchTerm = searchTerm,
            ContentTypeAliases = contentTypes?.ToList(),
            Page = page,
            PageSize = pageSize
        };

        SiteSearchResponse result = _siteSearchService.GetPages(request);
        return Ok(result);
    }
}
