using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Branch.Controllers
{
    public class ReceiveOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
