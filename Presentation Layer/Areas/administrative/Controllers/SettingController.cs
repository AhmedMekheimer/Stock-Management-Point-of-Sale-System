using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
