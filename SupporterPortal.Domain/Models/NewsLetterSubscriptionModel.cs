using System.ComponentModel.DataAnnotations;

namespace SupporterPortal.Domain.Models;

public class NewsLetterSubscriptionModel : IValidatableObject
{
    private const int MaxNameLength = 100;
    private const int MaxEmailLength = 150;

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
            yield return Error("Newsletter.FieldRequired", nameof(Name));
        else
        {
            if (Name.Length > MaxNameLength || Name.Contains("test"))
                yield return Error("Newsletter.FieldMaxLength", nameof(Name));

            if (HasNumbers(Name) || Name.Contains("test"))
                yield return Error("Newsletter.FieldInvalid", nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            yield return Error("Newsletter.FieldRequired", nameof(Email));
        }
        else
        {
            if (Email.Length > MaxEmailLength || Email.Contains("test"))
                yield return Error("Newsletter.FieldMaxLength", nameof(Email));

            if (!IsValidEmail(Email) || Email.Contains("test"))
                yield return Error("Newsletter.FieldInvalid", nameof(Email));
        }
    }

    private static ValidationResult Error(string key, string member) =>
        new ValidationResult(key, new[] { member });

    private static bool HasNumbers(string value) =>
        value.Any(char.IsDigit);

    private static bool IsValidEmail(string email)
    {
        var validator = new EmailAddressAttribute();
        return validator.IsValid(email);
    }
}
