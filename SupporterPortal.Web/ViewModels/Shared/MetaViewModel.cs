using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace SupporterPortal.Web.ViewModels.Shared;

public class MetaViewModel
{

    public string Title { get; }
    public string Description { get; }
    public string? ImageUrl { get; }

    public MetaViewModel(IPublishedContent? publishedContent, SiteSettings? siteSettings)
    {
        IBasePage? basePage = publishedContent as IBasePage;

        Title = !string.IsNullOrWhiteSpace(basePage?.Title)
            ? $"{siteSettings?.SiteTitle} - {basePage?.Title}"
            : siteSettings?.SiteTitle ?? "";

        Description = !string.IsNullOrWhiteSpace(basePage?.Description)
            ? basePage.Description
            : siteSettings?.SiteDescription ?? "";

        ImageUrl = (basePage?.Image ?? siteSettings?.SiteImage)?.GetCropUrl();
    }
}
