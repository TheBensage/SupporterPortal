using SupporterPortal.Application.Models.SiteSearch;

namespace SupporterPortal.Application.Services;
public interface ISiteSearchService
{
    SiteSearchResponse GetPages(SiteSearchRequest searchRequest);
}
