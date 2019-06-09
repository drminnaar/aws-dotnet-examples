using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;

namespace Cognito.MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;

        internal static readonly string Name = nameof(HomeController).Replace("Controller", "");

        public HomeController(SignInManager<CognitoUser> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(
                   actionName: nameof(DashboardController.Index),
                   controllerName: DashboardController.Name);
            }

            return View();
        }
    }
}
