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
        public async Task<IActionResult> Save(string id = "")
        {
            var allPermissions = await _UnitOfWork.Permissions.GetAsync();
            var roleVM = new RoleViewModel();

            if (string.IsNullOrEmpty(id))
            {
                roleVM.PermissionsTree = BuildPermissionTree(allPermissions, new List<int>());
                return View(roleVM);
            }

            var role = await _RoleManager.FindByIdAsync(id);
            var permissionsIds = _UnitOfWork.RolePermissions.GetAsync(p => p.RoleId == role.Id).GetAwaiter().GetResult().Select(p => p.PermissionId).ToList();

            roleVM.PermissionsTree = BuildPermissionTree(allPermissions, permissionsIds);
            roleVM.Name = role?.Name ?? "";
            roleVM.RoleId = id;
           
            return View(roleVM);
        }

        [HttpPost]
        [Authorize(Policy = "Role.Add|Role.Edit")]
        public async Task<IActionResult> Save(RoleViewModel RoleVM)
        {
            if (!ModelState.IsValid)
            {
                // reload permissions tree in case of error
                var allPermissions = await _UnitOfWork.Permissions.GetAsync();
                RoleVM.PermissionsTree = BuildPermissionTree(allPermissions, RoleVM.PermissionsIds ?? new List<int>());
                return View(RoleVM);
            }


            if(RoleVM.RoleId is null)
            {
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

                    if (!rolePermissionResult)
                    {
                        TempData["error"] = "Couldn't create role permissions.";
                        return View(RoleVM);
                    }
                }

                TempData["success"] = "Role Created Successfuly";
                return RedirectToAction(nameof(Index));

            } else
            {
        

                  var oldRolePermissions = _UnitOfWork.RolePermissions.GetAsync(r => r.RoleId == RoleVM.RoleId).GetAwaiter().GetResult();
                  var result = await _UnitOfWork.RolePermissions.DeleteRangeAsync(oldRolePermissions);

                  if(result)
                  {
                    if(RoleVM.PermissionsIds is not null) 
                    {
                        var rolePermissions = RoleVM.PermissionsIds.Select(pid => new RolePermission
                        {
                            RoleId = RoleVM.RoleId,
                            PermissionId = pid
                        });
                        var rolePermissionResult = await _UnitOfWork.RolePermissions.CreateRangeAsync(rolePermissions);

                    }

                    TempData["success"] = "Role Created Successfuly";
                      return RedirectToAction(nameof(Index));

                  }

           

            }
            TempData["success"] = "Somthing is wrong";
            return View(RoleVM);
        }

        [HttpPost]
        [Authorize(Policy = "Role.Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _RoleManager.FindByIdAsync(id) is IdentityRole Role)
            {

                var deleteResult = await _RoleManager.DeleteAsync(Role);
                if (deleteResult.Succeeded)
                {
                    TempData["Success"] = "Role Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Role";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Role Not Found";
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
