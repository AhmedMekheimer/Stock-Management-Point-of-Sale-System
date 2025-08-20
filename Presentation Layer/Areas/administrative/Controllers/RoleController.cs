using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Administrative.ViewModels;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("Administrative")]
    [Authorize(Roles = SD.SuperAdmin)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUnitOfWork _UnitOfWork;
        public RoleController(RoleManager<IdentityRole> roleManager, IUnitOfWork UnitOfWork)
        {
            _RoleManager = roleManager;
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            var roles = _RoleManager.Roles.ToList();
            return View(roles);
        }
    }
}
