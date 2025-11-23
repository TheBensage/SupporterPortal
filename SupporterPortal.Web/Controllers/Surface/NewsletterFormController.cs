namespace SupporterPortal.Web.Controllers.Surface;

using Microsoft.AspNetCore.Mvc;
using SupporterPortal.Web.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

public class NewsletterFormController : SurfaceController
{
    public NewsletterFormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider
    ) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit(NewsletterFormModel model)
    {
        if (!ModelState.IsValid)
        {
            Dictionary<string, string[]?> errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return Json(new { success = false, errors });
        }

        Console.WriteLine($"Newsletter signup: {model.Name} / {model.Email}");

        return Json(new { success = true, message = "Thanks for subscribing!" });
    }
}
