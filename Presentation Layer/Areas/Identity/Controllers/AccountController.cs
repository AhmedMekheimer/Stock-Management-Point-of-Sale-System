using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult SendEmail()
        {
            return View();
        }
    }
}
