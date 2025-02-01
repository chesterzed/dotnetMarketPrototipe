using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFirstProject.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MyFirstProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            ViewBag.DebagMessage = "Welcome to the login page";
            // Завершаем текущий сеанс работы, нужно чтобы не плодить куки
            await _signInManager.SignOutAsync();

            // раздел сайте (юрл), куда перенаправить пользователя после успешного входа
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            // раздел сайте (юрл), куда перенаправить пользователя после успешного входа
            ViewBag.ReturnUrl = returnUrl;

            ViewBag.DebagMessage = $"New login {Request.Form["model.UserName"]}  {Request.Form["LoginViewModel.Password"]}";

            if (!ModelState.IsValid)
                return View(model);

            SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);

            if (result.Succeeded)
                return Redirect(returnUrl ?? "/");

            ModelState.AddModelError(string.Empty, "Invalid login or password");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
