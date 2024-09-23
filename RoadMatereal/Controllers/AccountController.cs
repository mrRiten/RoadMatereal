using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoadMatereal.Models;
using RoadMatereal.ViewModels;
using System;
using System.Timers;

namespace RoadMatereal.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ILogger<AccountController> _logger = logger;

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmUrl = Url.Action("ConfirmEmail", "Account", new { token, UserId = user.Id }, Request.Scheme);

                    // Send to Redis
                    _logger.LogWarning("Send to user {Email} {ConfirmUrl}", user.Email, confirmUrl);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }
            else if (user.EmailConfirmed)
            {
                return BadRequest();
            }
            else
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return BadRequest();
        }
    }
}

