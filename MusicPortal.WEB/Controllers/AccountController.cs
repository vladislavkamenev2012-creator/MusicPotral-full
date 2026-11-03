using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MusicPortal.DAL.Interfaces;
using MusicPortal.WEB.Models;
using System.Security.Claims;

namespace MusicPortal.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AccountController(IUnitOfWork uow, IStringLocalizer<SharedResource> localizer)
        {
            _uow = uow;
            _localizer = localizer;
        }

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _uow.Users.Find(u => u.Username == model.Username && u.PasswordHash == model.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", _localizer["InvalidCredentials"].Value);
                    return View(model);
                }

                if (!user.IsApproved)
                {
                    ModelState.AddModelError("", _localizer["AccountNotApproved"].Value);
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Songs");
            }
            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _uow.Users.Find(u => u.Username == model.Username).FirstOrDefault();
                if (existing != null)
                {
                    ModelState.AddModelError("", _localizer["UserExists"].Value);
                    return View(model);
                }

                var user = new DAL.Entities.User
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Role = "Pending",
                    IsApproved = false
                };

                await _uow.Users.CreateAsync(user);
                await _uow.SaveChangesAsync();

                ViewBag.Message = _localizer["RegistrationSent"].Value;
                return View();
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Songs");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}
