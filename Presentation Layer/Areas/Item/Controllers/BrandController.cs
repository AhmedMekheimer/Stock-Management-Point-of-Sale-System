using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize(Policy = SD.Managers)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public BrandController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var brandsList = await _UnitOfWork.Brands.GetAsync();
            return View(brandsList);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id = 0)
        {
            var brandVM = new Brand();

            // Display Edit Page
            if (id == 0)
            {
                if ((await _UnitOfWork.Brands.GetOneAsync(b => b.Id == id)) is Brand brand)
                {
                    brandVM = brand;
                    return View(brandVM);
                }

                TempData["Error"] = "Brand Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(brandVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Brand brandVM)
        {
            if (brandVM.Id == 0)
            {
                var createResult = await _UnitOfWork.Brands.CreateAsync(brandVM);
                if (createResult)
                {
                    TempData["Success"] = "Brand Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Brand";
                return RedirectToAction(nameof(Index));
            }

            var updateResult = await _UnitOfWork.Brands.UpdateAsync(brandVM);
            if (updateResult)
            {
                TempData["Success"] = "Brand Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Error Updating Brand";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Brands.GetOneAsync(b => b.Id == id)) is Brand brand)
            {
                var deleteResult = await _UnitOfWork.Brands.DeleteAsync(brand);
                if (deleteResult)
                {
                    TempData["Success"] = "Brand Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Brand";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Brand Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
