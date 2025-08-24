using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize(Policy = SD.Managers)]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public SizeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var sizesList = await _UnitOfWork.Sizes.GetAsync();
            return View(sizesList);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id = 0)
        {
            var sizeVM = new Size();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
                {
                    sizeVM = size;
                    return View(sizeVM);
                }

                TempData["Error"] = "Size Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(sizeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Size sizeVM)
        {
            // Saving a Newly-Added Size
            if (sizeVM.Id == 0)
            {
                var createResult = await _UnitOfWork.Sizes.CreateAsync(sizeVM);
                if (createResult)
                {
                    TempData["Success"] = "Size Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Size";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Color
            var updateResult = await _UnitOfWork.Sizes.UpdateAsync(sizeVM);
            if (updateResult)
            {
                TempData["Success"] = "Size Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Error Updating Size";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
            {
                var deleteResult = await _UnitOfWork.Sizes.DeleteAsync(size);
                if (deleteResult)
                {
                    TempData["Success"] = "Size Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Size";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Color Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
