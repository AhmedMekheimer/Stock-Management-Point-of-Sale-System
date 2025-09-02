using CoreLayer;
using CoreLayer.Models;

using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.administrative.ViewModels;
using static PresentationLayer.Areas.administrative.ViewModels.RoleVM;
using System.Data;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("Administrative")]
    [Authorize]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUnitOfWork _UnitOfWork;
        public RoleController(RoleManager<IdentityRole> roleManager, IUnitOfWork UnitOfWork)
        {
            _RoleManager = roleManager;
            _UnitOfWork = UnitOfWork;
        }
        [Authorize(Policy = "Role.View")]
        public IActionResult Index()
        {
            var roles = _RoleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        [Authorize(Policy = "Role.Add|Role.Edit")]
        public async Task<IActionResult> Save(int? id = 0)
        {
            var roleVM = new RoleViewModel();

            var allPermissions = await  _UnitOfWork.Permissions.GetAsync();

            if(id == 0)
            {
                roleVM = new RoleViewModel()
                {
                    PermissionsTree = BuildPermissionTree(allPermissions, new List<int>())
                };
            }
     

            return View(roleVM);
        }

        [HttpPost]
        [Authorize(Policy = "Role.Add|Role.Edit")]
        public async Task<IActionResult> Save(RoleViewModel RoleVM )
        {
            if (!ModelState.IsValid)
            {
                // reload permissions tree in case of error
                var allPermissions = await _UnitOfWork.Permissions.GetAsync();
                RoleVM.PermissionsTree = BuildPermissionTree(allPermissions, RoleVM.PermissionsIds ?? new List<int>());
                return View(RoleVM);
            }

            // 1. Create the role
            var role = new IdentityRole(RoleVM.Name);
            var result = await _RoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                TempData["error"] = "Couldn't create Role.";
                return View(RoleVM);
            }

            // 2. Assign selected permissions
            if (RoleVM.PermissionsIds != null && RoleVM.PermissionsIds.Any())
            {
                var rolePermissions = RoleVM.PermissionsIds.Select(pid => new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = pid
                });

              var rolePermissionResult = await _UnitOfWork.RolePermissions.CreateRangeAsync(rolePermissions);

                if(!rolePermissionResult)
                {
                    TempData["error"] = "Couldn't create role permissions.";
                    return View(RoleVM);
                }
            }

            return View(RoleVM);
        }

        [HttpPost]
        [Authorize(Policy = "Role.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == id)) is Tax tax)
            {
                var deleteResult = await _UnitOfWork.Taxes.DeleteAsync(tax);
                if (deleteResult)
                {
                    TempData["Success"] = "Tax Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Tax";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Tax Not Found";
            return RedirectToAction(nameof(Index));
        }





        // Helper: Build tree
        [NonAction]
        private List<PermissionTreeViewModel> BuildPermissionTree(List<Permission> allPermissions,
            List<int> assignedIds,
            int? parentId = null)
        {
            return allPermissions
                .Where(p => p.ParentId == parentId)
                .Select(p => new PermissionTreeViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    EnglishName = p.EnglishName,
                    IsAssigned = assignedIds.Contains(p.Id),
                    Children = BuildPermissionTree(allPermissions, assignedIds, p.Id)
                }).ToList();
        }
    }
}
