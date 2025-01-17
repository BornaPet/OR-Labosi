using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace OtvorenoRacunalstvoLabosi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "/")
        {
            var redirectUri = Url.Action("Index", "Home");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, "Auth0");
        }

        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") },
                CookieAuthenticationDefaults.AuthenticationScheme,
                "Auth0"
            );
        }
    }
}
