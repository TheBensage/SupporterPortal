using Umbraco.Cms.Web.Common.PublishedModels;

namespace SupporterPortal.Web.ViewModels.Shared;

public class NavigationViewModel
{
    public NavigationViewModel(SiteSettings? siteSettings)
    {
        SiteTitle = siteSettings?.SiteTitle ?? "Site";
        MainNavigationItems = siteSettings?.MainNavigationItems?
            .Select(LinkViewModel.FromUmbracoLink) ?? [];

        LogoutLink = new LinkViewModel(siteSettings?.LogoutText ?? "Logout", "/api/logout");

        LoginLink = new LinkViewModel(siteSettings?.LoginText ?? "Login", "/api/login");

    }

    public string SiteTitle { get; set; }

    public IEnumerable<LinkViewModel> MainNavigationItems { get; set; }

    public LinkViewModel LogoutLink { get; set; }

    public LinkViewModel LoginLink { get; set; }
}
