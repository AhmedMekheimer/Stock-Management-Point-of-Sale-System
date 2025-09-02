using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Branch.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.Branch.Controllers
{

    [Area("Branch")]
    [Authorize]
    public class BranchController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;


        public BranchController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> Index()
        {
            var branches = await _UnitOfWork.Branches.GetAsync(include: [x => x.BranchManager]);

            return View(branches);
        }
        [HttpGet]
        [Authorize(Policy = "Stock.Add|Stock.Edit")]
        public async Task<IActionResult> Save(int? Id)
        {
            var branchVM = new BranchVM()
            {
                UsersList = _UserManager.Users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })

            };
            var branch = await _UnitOfWork.Branches.GetOneAsync(x => x.Id == Id);

            if (branch is not null)
            {
                branchVM.BranchManagerId = branch.BranchManagerId;
                branchVM.Name = branch.Name;
                branchVM.BranchId = branch.Id;
                return View(branchVM);
            }

            return View(branchVM);
        }
        [HttpPost]
        [Authorize(Policy = "Stock.Add|Stock.Edit")]
        public async Task<IActionResult> Save(BranchVM branchVM)
        {
            var user = await _UserManager.FindByIdAsync(branchVM.BranchManagerId);
            branchVM.UsersList = _UserManager.Users.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName });
            if (user is not null)
            {

                if (branchVM.BranchId is not null)
                {
                    var oldBranch = await _UnitOfWork.Branches.GetOneAsync(b => b.Id == branchVM.BranchId);
                    if (oldBranch is not null)
                    {

                        oldBranch.Name = branchVM.Name;

                        if (oldBranch.BranchManagerId != branchVM.BranchManagerId)
                        {

                            var branchUsers = await _UnitOfWork.Branches.GetOneAsync(b => b.BranchManagerId == branchVM.BranchManagerId);

                            if (branchUsers is null)
                            {
                                //var oldUser = await _UserManager.FindByIdAsync(oldBranch.BranchManagerId);
                                //user.BranchId = oldBranch.Id;
                                //oldUser.BranchId = null;

                                oldBranch.BranchManagerId = branchVM.BranchManagerId;
                                var resultBranchManager = await _UnitOfWork.Branches.UpdateAsync(oldBranch);
                                //var updatedUserRes = await _UserManager.UpdateAsync(user);
                                //var oldUserRes = await _UserManager.UpdateAsync(oldUser);
                                if (resultBranchManager)
                                {
                                    TempData["success"] = "Branch updated";
                                    return RedirectToAction(nameof(Index));
                                }
                            }
                            else
                            {
                                TempData["error"] = "User is on a branch";
                                return View(branchVM);
                            }

                        }
                        var result = await _UnitOfWork.Branches.UpdateAsync(oldBranch);
                        if (result)
                        {
                            TempData["success"] = "Branch updated";
                            return RedirectToAction(nameof(Index));
                        }
                    }

                }
                else
                {
                    var oldUserWithBranch = await _UnitOfWork.Branches.GetOneAsync(b => b.BranchManagerId == user.Id);

                    if (oldUserWithBranch is not null)
                    {
                        TempData["error"] = "User already on a branch";
                        return View(branchVM);
                    }

                    var newBranch = branchVM.Adapt<CoreLayer.Models.Branch>();

                    var result = await _UnitOfWork.Branches.CreateAsync(newBranch);

                    if (result)
                    {

                       var items = await _UnitOfWork.Items.GetAsync();
                        var branchItems = new List<BranchItem>();
                        if (items.Count() > 0)
                        {
                            foreach (var item in items)
                            {
                                var branchItem = new BranchItem()
                                {
                                    Quantity = 0,
                                    SellingPrice = 0,
                                    BuyingPriceAvg = 0,
                                    LastBuyingPrice = 0,
                                    ItemId = item.Id,
                                    BranchId = newBranch.Id
                                };
                                branchItems.Add(branchItem);
                            }


                          var resultAddItemBranch =   await _UnitOfWork.BranchItems.CreateRangeAsync(branchItems);

                            if (!resultAddItemBranch)
                            {
                                TempData["success"] = "Couldn't add item branch";
                                return RedirectToAction(nameof(Index));
                            }
                        }

                        //user.BranchId = newBranch.Id;
                        //await _UserManager.UpdateAsync(user);
                        TempData["success"] = "Branch added";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "Stock.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _UnitOfWork.Branches.GetOneAsync(b => b.Id == id);

            if (branch is not null)
            {

                var branchResult = await _UnitOfWork.Branches.DeleteAsync(branch);

                if (branchResult)
                {
                    TempData["success"] = "user delete.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
