namespace SupporterPortal.Application.Models.SiteSearch;
public record SiteSearchRequest
{
    public string? SearchTerm { get; set; }
    public List<string>? ContentTypeAliases { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;
    public string? OrderBy { get; set; }
}

