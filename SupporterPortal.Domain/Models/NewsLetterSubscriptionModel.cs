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
        // Name validations
        if (string.IsNullOrWhiteSpace(Name))
            yield return Error("Newsletter.NameRequired", nameof(Name));
        else
        {
            if (Name.Length > MaxNameLength)
                yield return Error("Newsletter.NameTooLong", nameof(Name));

            if (HasNumbers(Name))
                yield return Error("Newsletter.NameNumbersNotAllowed", nameof(Name));
        }

        // Email validations
        if (string.IsNullOrWhiteSpace(Email))
        {
            yield return Error("Newsletter.EmailRequired", nameof(Email));
        }
        else
        {
            if (Email.Length > MaxEmailLength)
                yield return Error("Newsletter.EmailTooLong", nameof(Email));

            if (!IsValidEmail(Email))
                yield return Error("Newsletter.EmailInvalidFormat", nameof(Email));
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
