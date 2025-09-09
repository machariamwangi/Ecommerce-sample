using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.RegisterAsync(model);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "" });

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _userService.LoginAsync(model);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "Customer" });

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }




    }
}
