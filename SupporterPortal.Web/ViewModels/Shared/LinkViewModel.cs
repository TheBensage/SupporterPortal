using Umbraco.Cms.Core.Models;

namespace SupporterPortal.Web.ViewModels.Shared;

public class LinkViewModel
{
    public string Name { get; }
    public string Url { get; }

    public LinkViewModel(string name, string url)
    {
        Name = name;
        Url = url;
    }
    public static LinkViewModel FromUmbracoLink(Link link)
        => new LinkViewModel(link.Name ?? "", link.Url ?? "#");
}
