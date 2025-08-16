using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    public class HomeController : Controller
    {
        [Authorize(Policy = $"{SD.Managers}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
