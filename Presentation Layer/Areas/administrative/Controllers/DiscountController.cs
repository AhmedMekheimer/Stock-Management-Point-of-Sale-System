using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Branch.ViewModels;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        [HttpGet]
        [Authorize(Policy = "Discount.View")]
        public async Task<IActionResult> Index()
        {
            var discountsList = await _UnitOfWork.Discounts.GetAsync();
            return View(discountsList);
        }

        [HttpGet]
        [Authorize(Policy = "Discount.Add|Discount.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var discountVM = new Discount();
            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == id)) is Discount discount)
                {
                    return View(discount);
                }

                TempData["Error"] = "Discount Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(discountVM);
        }

        [HttpPost]
        [Authorize(Policy = "Discount.Add|Discount.Edit")]
        public async Task<IActionResult> Save(Discount discountVM)
        {
            if (!ModelState.IsValid)
                return View(discountVM);

            // Saving a Newly-Added Tax
            if (discountVM.Id == 0)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Discounts.GetOneAsync(e => e.Name == discountVM.Name) is Discount))
                {
                    ModelState.AddModelError(nameof(discountVM.Name), "Name already exists");
                    return View(discountVM);
                }
                var createResult = await _UnitOfWork.Discounts.CreateAsync(discountVM);
                if (createResult)
                {
                    TempData["Success"] = "Discount Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Discount";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Tax
            if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == discountVM.Id, null, tracked: false)) is Discount)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Discounts.GetOneAsync(e => e.Name == discountVM.Name && e.Id != discountVM.Id) is Discount))
                {
                    ModelState.AddModelError(nameof(discountVM.Name), "Name already exists");
                    return View(discountVM);
                }
                var updateResult = await _UnitOfWork.Discounts.UpdateAsync(discountVM);
                if (updateResult)
                {
                    TempData["Success"] = "Discount Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Discount";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Discount Not Found";
            return View(discountVM);
        }
        [HttpPost]
        [Authorize(Policy = "Discount.AddOrEdit")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == id)) is Discount discount)
            {
                var deleteResult = await _UnitOfWork.Discounts.DeleteAsync(discount);
                if (deleteResult)
                {
                    TempData["Success"] = "Discount Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Discount";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Discount Not Found";
            return RedirectToAction(nameof(Index));
        }
    }

}
