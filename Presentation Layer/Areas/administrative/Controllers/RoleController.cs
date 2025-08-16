using CoreLayer.Models;
using Infrastructure_Layer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.administrative.ViewModels;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _RoleManager;

        public RoleController(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _RoleManager = roleManager;

        }
        public IActionResult Index()
        {
            var roles = _RoleManager.Roles.ToList();
            return View(roles);
        }
    }
}
