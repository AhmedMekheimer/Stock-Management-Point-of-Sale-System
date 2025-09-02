using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class TaxController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public TaxController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "Tax.View")]
        public async Task<IActionResult> Index()
        {
            var taxesList = await _UnitOfWork.Taxes.GetAsync();
            return View(taxesList);
        }

        [HttpGet]
        [Authorize(Policy = "Tax.Add|Tax.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var taxVM = new Tax();
            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == id)) is Tax tax)
                {
                    return View(tax);
                }

                TempData["Error"] = "Tax Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(taxVM);
        }


        [HttpPost]
        [Authorize(Policy = "Tax.Add|Tax.Edit")]
        public async Task<IActionResult> Save(Tax taxVM)
        {
            if (!ModelState.IsValid)
                return View(taxVM);

            // Saving a Newly-Added Tax
            if (taxVM.Id == 0)
            {
                var createResult = await _UnitOfWork.Taxes.CreateAsync(taxVM);
                if (createResult)
                {
                    TempData["Success"] = "Tax Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Tax";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Tax
            if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == taxVM.Id, null, tracked: true)) is Tax)
            {
                var updateResult = await _UnitOfWork.Taxes.UpdateAsync(taxVM);
                if (updateResult)
                {
                    TempData["Success"] = "Tax Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Updating Tax";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Tax Not Found";
            return View(taxVM);
        }


        [HttpPost]
        [Authorize(Policy = "Tax.Delete")]
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
    }
}
