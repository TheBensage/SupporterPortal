namespace SupporterPortal.Application.Models.SiteSearch;

public record SiteSearchResult
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public required string Url { get; set; }
    public required string ContentType { get; set; }
    public string? ImageUrl { get; set; }
}
