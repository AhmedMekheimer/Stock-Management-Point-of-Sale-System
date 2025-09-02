using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public SizeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [Authorize(Policy = "Size.View")]
        public async Task<IActionResult> Index()
        {
            var sizesList = await _UnitOfWork.Sizes.GetAsync();
            return View(sizesList);
        }

        [HttpGet]
        [Authorize(Policy = "Size.Add|Size.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var sizeVM = new ItemVariantVM<Size>();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
                {
                    sizeVM.ItemVariant = size;
                    return View(sizeVM);
                }

                TempData["Error"] = "Size Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(sizeVM);
        }

        [HttpPost]
        [Authorize(Policy = "Size.Add|Size.Edit")]
        public async Task<IActionResult> Save(ItemVariantVM<Size> sizeVM)
        {
            if (!ModelState.IsValid)
                return View(sizeVM);

            Result result = new Result();

            if (sizeVM.ItemVariant.Id == 0) // Create
            {
                if (sizeVM.formFile != null)
                {
                    result = ImageService.UploadNewImage(sizeVM.formFile);
                    if (result.Success)
                        sizeVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(sizeVM);
                    }
                }

                var createResult = await _UnitOfWork.Sizes.CreateAsync(sizeVM.ItemVariant);
                if (createResult)
                {
                    TempData["Success"] = "Size Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Size";
                return RedirectToAction(nameof(Index));
            }

            // Update existing
            if ((await _UnitOfWork.Sizes.GetOneAsync(s => s.Id == sizeVM.ItemVariant.Id)) is Size size)
            {
                if (sizeVM.formFile != null) // Replace
                {
                    if (size.Image != null)
                    {
                        result = ImageService.DeleteImage(size.Image);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(sizeVM);
                        }
                    }
                    result = ImageService.UploadNewImage(sizeVM.formFile);
                    if (result.Success)
                        sizeVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(sizeVM);
                    }
                }
                else
                {
                    if (sizeVM.deleteImage) // Delete old
                    {
                        if (size.Image != null)
                        {
                            result = ImageService.DeleteImage(size.Image);
                            if (!result.Success)
                            {
                                TempData["Error"] = result.ErrorMessage;
                                return View(sizeVM);
                            }
                        }
                    }
                    else
                        sizeVM.ItemVariant.Image = size.Image; // Keep
                }

                _UnitOfWork.Sizes.DetachEntity(size);
                var updateResult = await _UnitOfWork.Sizes.UpdateAsync(sizeVM.ItemVariant);
                if (updateResult)
                {
                    TempData["Success"] = "Size Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Updating Size";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Size Not Found";
            return View(sizeVM);
        }
        [HttpPost]
        [Authorize(Policy = "Size.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result();
            if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
            {
                if (size.Image != null)
                {
                    // Deleting Image Physically
                    result = ImageService.DeleteImage(size.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }

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
