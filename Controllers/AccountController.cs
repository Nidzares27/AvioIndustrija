using AvioIndustrija.Data;
using AvioIndustrija.Models;
using AvioIndustrija.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet.Actions;

namespace AvioIndustrija.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AvioIndustrija2424Context _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AvioIndustrija2424Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login([FromQuery] string? returnUrl)
        {
            var response = new LoginViewModel()
            {
                returnUrl = returnUrl
            };
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginViewModel)
        {
            if (!ModelState.IsValid) return View(LoginViewModel);

            var user = await _userManager.FindByEmailAsync(LoginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, LoginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, LoginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(LoginViewModel.returnUrl))
                        {
                            return LocalRedirect(LoginViewModel.returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Avioni");
                        }
                    }
                }
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(LoginViewModel);
            }
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(LoginViewModel);

        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Ime = registerViewModel.Ime,
                Prezime = registerViewModel.Prezime,
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    
                }
                return View();
            }
            return RedirectToAction("Login", "Account");

        }
        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
