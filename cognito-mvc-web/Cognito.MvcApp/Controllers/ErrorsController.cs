using Microsoft.AspNetCore.Mvc;

namespace Cognito.MvcApp.Controllers
{
    public sealed class ErrorsController : Controller
    {
        internal static readonly string Name = nameof(ErrorsController).Replace("Controller", string.Empty);

        [Route("errors/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }        
    }
}