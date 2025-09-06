using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Branch.ViewModels;

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
            var branches = await _UnitOfWork.Branches.GetAsync();

            return View(branches);
        }
        [HttpGet]
        [Authorize(Policy = "Stock.Add|Stock.Edit")]
        public async Task<IActionResult> Save(int? Id)
        {
            var branchVM = new BranchVM();
            var branch = await _UnitOfWork.Branches.GetOneAsync(x => x.Id == Id);

            if (branch is not null)
            {
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

            if (branchVM.BranchId is not null)
            {
                var oldBranch = await _UnitOfWork.Branches.GetOneAsync(b => b.Id == branchVM.BranchId);
                if (oldBranch is not null)
                {
                    // Checking Name Uniqueness
                    if ((await _UnitOfWork.Branches.GetOneAsync(e => e.Name == branchVM.Name && e.Id != branchVM.BranchId) is CoreLayer.Models.Branch))
                    {
                        ModelState.AddModelError(nameof(branchVM.Name), "Name already exists");
                        return View(branchVM);
                    }
                    oldBranch.Name = branchVM.Name;
                    var result = await _UnitOfWork.Branches.CommitAsync();
                    if (result)
                    {
                        TempData["success"] = "Branch updated";
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            else
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Branches.GetOneAsync(e => e.Name == branchVM.Name && e.Id != branchVM.BranchId) is CoreLayer.Models.Branch))
                {
                    ModelState.AddModelError(nameof(branchVM.Name), "Name already exists");
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


                        var resultAddItemBranch = await _UnitOfWork.BranchItems.CreateRangeAsync(branchItems);

                        if (!resultAddItemBranch)
                        {
                            TempData["success"] = "Couldn't add item branch";
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    TempData["success"] = "Branch added";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Branch";
                return RedirectToAction(nameof(Index));
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
