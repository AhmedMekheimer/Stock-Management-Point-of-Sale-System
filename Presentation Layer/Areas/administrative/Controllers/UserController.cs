using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
