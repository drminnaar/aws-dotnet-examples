using System;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Cognito.MvcApp.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApp.Controllers
{
    public sealed class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;

        public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signIn)
        {
            if (!ModelState.IsValid)
                return View(signIn);

            var user = await _userManager.FindByEmailAsync(signIn.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"A user having email '{signIn.Email}' does not exist.");
                return View(signIn);
            }

            var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isConfirmed)
            {
                ModelState.AddModelError(string.Empty, $"Your account having email '{signIn.Email}' has not yet been confirmed.");
                return View(signIn);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                signIn.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials provided");
                return View(signIn);
            }

            return RedirectToAction(nameof(DashboardController.Index), DashboardController.Name);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(SignIn));
            }

            return RedirectToAction(nameof(DashboardController.Index), DashboardController.Name);
        }
    }
}