using System.ComponentModel.DataAnnotations;

namespace SupporterPortal.Web.Models;

public class NewsletterFormModel
{
    [Required(ErrorMessage = "Please enter your name.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Please enter your email.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email.")]
    public required string Email { get; set; }
}
