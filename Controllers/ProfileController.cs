using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OtvorenoRacunalstvoLabosi.DTO;
using System.Security.Claims;

namespace OtvorenoRacunalstvoLabosi.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userData = claimsIdentity.Claims.Select(c => new ArtistDto {Name = c.Type, Nationality = c.Value }).ToList();

            return View(userData);
        }
    }
}
