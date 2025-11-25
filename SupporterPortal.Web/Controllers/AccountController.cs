using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SupporterPortal.Web.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "Auth0");
    }

    [HttpGet("logout")]
    public IActionResult Logout(string returnUrl = "/")
    {
        return SignOut(
            new AuthenticationProperties { RedirectUri = returnUrl },
            CookieAuthenticationDefaults.AuthenticationScheme,
            "Auth0");
    }
}
