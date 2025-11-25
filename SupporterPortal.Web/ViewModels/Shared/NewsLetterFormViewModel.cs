using Umbraco.Cms.Web.Common.PublishedModels;

namespace SupporterPortal.Web.ViewModels.Shared;

public class NewsLetterFormViewModel
{
    public string Title { get; }

    public string Description { get; }
    public string NameLabel { get; }

    public string EmailLabel { get; }

    public string NamePlaceholder { get; }

    public string EmailPlaceholder { get; }

    public string SubmitText { get; }

    public string ErrorText { get; }
    public NewsLetterFormViewModel(SiteSettings? siteSettings)
    {
        NewsLetterForm? newsletterFormBlock = siteSettings?.SiteNewsletterForm?.Content;
        Title = newsletterFormBlock?.Title ?? "Subscribe to our newsletter";
        Description = newsletterFormBlock?.Description ?? "Stay updated with our latest news";
        SubmitText = newsletterFormBlock?.SubmitText ?? "Subscribe";
        ErrorText = newsletterFormBlock?.ErrorText ?? string.Empty;
        NameLabel = newsletterFormBlock?.NameLabelText ?? "Name";
        EmailLabel = newsletterFormBlock?.EmailLabelText ?? "Email";
        NamePlaceholder = newsletterFormBlock?.NamePlaceholder ?? "Enter your name";
        EmailPlaceholder = newsletterFormBlock?.EmailPlaceholder ?? "Enter your email";
    }
}
