using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewData["Message"] = "AccessDenied.";
            ViewData["ReturnUrl"] = ReturnUrl;

            return View();
        }
    }
}