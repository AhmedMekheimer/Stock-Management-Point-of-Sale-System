using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json.Linq;
using PresentationLayer.Areas.Administrative.ViewModels;
using System.Formats.Asn1;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("Administrative")]
    [Authorize(Roles = SD.SuperAdmin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUnitOfWork _UnitOfWork;

        public UserController(UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager, IUnitOfWork UnitOfWork)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _UnitOfWork = UnitOfWork;

        }
        public IActionResult Index()
        {
            var user = _UserManager.Users.ToList();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Save(string? id)
        {
            var createUser = new CreateUser();

            var branches = await _UnitOfWork.Branches.GetAsync();

            createUser.BranchList = branches
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();

            createUser.RolesList = _RoleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            if (id is not null)
            {
                var user = _UserManager.Users.FirstOrDefault(x => x.Id == id);

                if (user != null)
                {
                    ViewBag.Id = user.Id;

                    var branchId = _UnitOfWork.Branches.GetOneAsync((b => b.Id == user.BranchId)).Id;
                    var roleId = await _UserManager.GetRolesAsync(user);
                    ViewBag.UserId = user.Id;
                    createUser.UserName = user.UserName ?? "";
                    createUser.Email = user.Email ?? "";
                    createUser.BranchId = branchId;
                    createUser.RoleId = roleId.FirstOrDefault() ?? "";



                    return View(createUser);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(createUser);
        }


        [HttpPost]
        public async Task<IActionResult> Save(CreateUser createUser)
        {
            try
            {
                var branches = await _UnitOfWork.Branches.GetAsync();

                createUser.BranchList = branches
                    .Select(b => new SelectListItem
                    {
                        Value = b.Id.ToString(), Text = b.Name 
                    })
                    .ToList();

                createUser.RolesList = _RoleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

                var branchId = createUser.BranchId == 0 ? null : createUser.BranchId;
                ApplicationUser user = new()
                {
                    UserName = createUser.UserName,
                    Email = createUser.Email,
                    EmailConfirmed = true,
                    BranchId = branchId
                };

                if (createUser.UserId is null)
                {
                    var result = await _UserManager.CreateAsync(user, createUser.Password);
                    if (result.Succeeded)
                    {
                        var userDb = await _UserManager.FindByNameAsync(user.UserName);

                        if (userDb is not null)
                        {

                            if (createUser.RoleId != "0")
                            {

                                var role = await _UserManager.AddToRoleAsync(userDb, createUser.RoleId);

                                if (role.Succeeded)
                                {

                                    TempData["success"] = "User added";
                                    return RedirectToAction(nameof(Index));
                                }
                            }

                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));

                        return View(createUser);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {


                    var oldUser = await _UserManager.FindByIdAsync(createUser.UserId);

                    var UserUpdated = createUser.Adapt(oldUser);


                    if (createUser.BranchId == 0)
                    {
                        UserUpdated.BranchId = null;
                    }
                    else
                    {
                        UserUpdated.BranchId = createUser.BranchId;
                    }



                    var newResult = await _UserManager.UpdateAsync(UserUpdated);

                    if (newResult.Succeeded)
                    {

                        var userRole = await _UserManager.GetRolesAsync(UserUpdated);

                        if (userRole.Count() > 0)
                        {
                            await _UserManager.RemoveFromRoleAsync(UserUpdated, userRole.FirstOrDefault() ?? "");

                            if (createUser.RoleId != "0")
                                await _UserManager.AddToRoleAsync(UserUpdated, createUser.RoleId);

                        }
                        else
                        {
                            if (createUser.RoleId != "0")
                                await _UserManager.AddToRoleAsync(UserUpdated, createUser.RoleId);
                        }

                        TempData["success"] = "updated";
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                var x = ex.Message;

                return View();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);

            if (user is not null)
            {
                var isUserBranchManager = await _UnitOfWork.Branches.AnyAsync(b => b.BranchManagerId == id);

                if (isUserBranchManager)
                {
                    TempData["error"] = "Change branch manager to delete user.";
                    return RedirectToAction(nameof(Index));
                }

                var userResult = await _UserManager.DeleteAsync(user);

                if (userResult.Succeeded)
                {
                    TempData["success"] = "user delete.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
