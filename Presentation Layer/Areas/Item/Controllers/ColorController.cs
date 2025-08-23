using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize(Policy = SD.Managers)]
    public class ColorController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ColorController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var colorsList = await _UnitOfWork.Colors.GetAsync();
            return View(colorsList);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id = 0)
        {
            var colorVM = new Color();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Colors.GetOneAsync(b => b.Id == id)) is Color color)
                {
                    colorVM = color;
                    return View(colorVM);
                }

                TempData["Error"] = "Color Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(colorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Color colorVM)
        {
            // Saving a Newly-Added Color
            if (colorVM.Id == 0)
            {
                var createResult = await _UnitOfWork.Colors.CreateAsync(colorVM);
                if (createResult)
                {
                    TempData["Success"] = "Color Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Color";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Color
            var updateResult = await _UnitOfWork.Colors.UpdateAsync(colorVM);
            if (updateResult)
            {
                TempData["Success"] = "Color Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Error Updating Color";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Colors.GetOneAsync(b => b.Id == id)) is Color color)
            {
                var deleteResult = await _UnitOfWork.Colors.DeleteAsync(color);
                if (deleteResult)
                {
                    TempData["Success"] = "Color Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Color";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Color Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
