using System;
using System.Text;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Cognito.MvcApp.ViewModels.Signups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApp.Controllers
{
    public sealed class SignupsController : Controller
    {
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public SignupsController(UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel signup)
        {
            if (!ModelState.IsValid)
                return View(signup);

            var cognitoUser = _pool.GetUser(signup.Email);

            if (cognitoUser == null)
                throw new InvalidOperationException("Expected a non-null Cognito user to be returned.");

            if (!string.IsNullOrWhiteSpace(cognitoUser.Status))
            {
                ModelState.AddModelError(string.Empty, $"A user having email '{signup.Email}' already exists.");
                return View(signup);
            }

            cognitoUser.Attributes.Add(CognitoAttribute.Name.AttributeName, signup.Name);
            cognitoUser.Attributes.Add(CognitoAttribute.Email.AttributeName, signup.Email);

            var identityResult = await _userManager.CreateAsync(cognitoUser, signup.Password);

            if (!identityResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"A user having email '{signup.Email}' could not be created.");

                foreach (var error in identityResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(signup);
            }

            return RedirectToAction(nameof(Confirm), new { email = signup.Email });
        }

        [HttpGet]
        public IActionResult Confirm(string email)
        {
            return View(new SignupConfirmationViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(SignupConfirmationViewModel confirmation)
        {
            if (!ModelState.IsValid)
                return View(confirmation);

            var user = await _userManager.FindByEmailAsync(confirmation.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"A user having email '{confirmation.Email}' does not exist.");
                return View(confirmation);
            }

            var confirmationResult = await ((CognitoUserManager<CognitoUser>)_userManager)
                .ConfirmSignUpAsync(user, confirmation.Code, true);

            if (!confirmationResult.Succeeded)
            {
                foreach (var error in confirmationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(confirmation);
            }

            return RedirectToAction(nameof(Complete));
        }

        [HttpGet]
        public IActionResult Complete()
        {
            return View();
        }
    }
}
