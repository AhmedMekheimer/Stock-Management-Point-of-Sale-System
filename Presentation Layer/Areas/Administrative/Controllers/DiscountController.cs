using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize(Policy = SD.Managers)]
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var discountsList = await _UnitOfWork.Discounts.GetAsync();
            return View(discountsList);
        }

        [HttpGet]
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
        public async Task<IActionResult> Save(Discount discountVM)
        {
            if (!ModelState.IsValid)
                return View(discountVM);

            // Saving a Newly-Added Tax
            if (discountVM.Id == 0)
            {
                var createResult = await _UnitOfWork.Discounts.CreateAsync(discountVM);
                if (createResult)
                {
                    TempData["Success"] = "Discount Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Discount";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Tax
            if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == discountVM.Id, null, tracked: true)) is Discount)
            {
                var updateResult = await _UnitOfWork.Discounts.UpdateAsync(discountVM);
                if (updateResult)
                {
                    TempData["Success"] = "Discount Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Updating Discount";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Discount Not Found";
            return View(discountVM);
        }

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
