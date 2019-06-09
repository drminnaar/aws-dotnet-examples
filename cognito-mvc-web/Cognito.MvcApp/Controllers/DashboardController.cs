using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApp.Controllers
{
    [Authorize]
    public sealed class DashboardController : Controller
    {
        internal static readonly string Name = nameof(DashboardController).Replace("Controller", "");

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }        
    }
}