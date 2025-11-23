namespace SupporterPortal.Application.Models.SiteSearch;

public record SiteSearchResponse
{
    public IEnumerable<SiteSearchResult> Results { get; set; } = new List<SiteSearchResult>();
    public int TotalCount { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
