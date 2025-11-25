using Umbraco.Cms.Web.Common.PublishedModels;

namespace SupporterPortal.Web.ViewModels.Shared;

public class SiteSettingsViewModel
{
    public string SiteTitle { get; }
    public string SiteDescription { get; }
    public string? ImageUrl { get; }

    public NavigationViewModel Navigation { get; }

    public SiteSettingsViewModel(SiteSettings siteSettings)
    {
        SiteTitle = siteSettings.SiteTitle ?? "";
        SiteDescription = siteSettings.SiteDescription ?? "";
        ImageUrl = siteSettings.SiteImage?.GetCropUrl();
        Navigation = new NavigationViewModel(siteSettings);
    }
}

