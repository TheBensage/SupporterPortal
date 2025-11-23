using Microsoft.AspNetCore.Mvc;
using SupporterPortal.Application.Models.Crm;
using SupporterPortal.Application.Services;
using SupporterPortal.Domain.Models;
using Umbraco.Cms.Web.Common;

namespace SupporterPortal.Web.Controllers;

public record NewsLetterFormResponse
{
    public bool Success { get; set; }

    public Dictionary<string, string[]?>? Errors { get; set; } = null;

    public string? Message { get; set; }
}

[ApiController]
public class NewsLetterFormController : Controller
{
    private readonly ICrmService _crmService;
    private readonly UmbracoHelper _umbracoHelper;

    public NewsLetterFormController(ICrmService crmService, IUmbracoHelperAccessor umbracoHelperAccessor)
    {
        _crmService = crmService ?? throw new ArgumentNullException(nameof(crmService));

        if (!umbracoHelperAccessor.TryGetUmbracoHelper(out var helper) || helper is null)
            throw new InvalidOperationException("Could not get UmbracoHelper");

        _umbracoHelper = helper;
    }

    [HttpPost("/api/subscribe")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromForm] NewsLetterSubscriptionModel model)
    {
        if (!ModelState.IsValid)
        {
            Dictionary<string, string[]?> errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors
                    .Select(e => _umbracoHelper.GetDictionaryValueOrDefault(e.ErrorMessage, e.ErrorMessage))
                    .ToArray()
            );

            return Json(new NewsLetterFormResponse
            {
                Success = false,
                Errors = errors
            });
        }

        CrmRecord? existing = await _crmService.GetRecordByEmailAsync(model.Email);
        if (existing != null)
        {
            return Json(new NewsLetterFormResponse
            {
                Success = true,
                Message = _umbracoHelper.GetDictionaryValueOrDefault(
                    "Newsletter.AlreadySubscribed",
                    "You are already subscribed."
                )
            });
        }

        CrmRecord newRecord = new()
        {
            Name = model.Name,
            Email = model.Email
        };

        await _crmService.SendRecordAsync(newRecord);

        return Json(new NewsLetterFormResponse
        {
            Success = true,
            Message = _umbracoHelper.GetDictionaryValueOrDefault(
                "Newsletter.SuccessMessage",
                "Thanks for subscribing!"
            )
        });
    }
}
