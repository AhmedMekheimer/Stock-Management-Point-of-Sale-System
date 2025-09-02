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
    public class TargetAudienceController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;

        public TargetAudienceController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        [Authorize(Policy = "TargetAudience.View")]
        public async Task<IActionResult> Index()
        {
            var TargetAudiencesList = await _UnitOfWork.TargetAudiences.GetAsync();
            return View(TargetAudiencesList);
        }

        [HttpGet]
        [Authorize(Policy = "TargetAudience.Add|TargetAudience.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var TargetAudienceVM = new ItemVariantVM<TargetAudience>();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.TargetAudiences.GetOneAsync(b => b.Id == id)) is TargetAudience targetAudience)
                {
                    TargetAudienceVM.ItemVariant = targetAudience;
                    return View(TargetAudienceVM);
                }

                TempData["Error"] = "Target Audience Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(TargetAudienceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "TargetAudience.Add|TargetAudience.Edit")]
        public async Task<IActionResult> Save(ItemVariantVM<TargetAudience> taVM)
        {
            if (!ModelState.IsValid)
                return View(taVM);

            Result result = new Result(); ;

            if (taVM.ItemVariant.Id == 0) // Create
            {
                if (taVM.formFile != null)
                {
                    result = ImageService.UploadNewImage(taVM.formFile);
                    if (result.Success)
                        taVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(taVM);
                    }
                }

                var createResult = await _UnitOfWork.TargetAudiences.CreateAsync(taVM.ItemVariant);
                if (createResult)
                {
                    TempData["Success"] = "Target audience added successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Error adding target audience";
                return RedirectToAction(nameof(Index));
            }

            // Update
            var existing = await _UnitOfWork.TargetAudiences.GetOneAsync(t => t.Id == taVM.ItemVariant.Id);
            if (existing is null)
            {
                TempData["Error"] = "Target audience not found";
                return RedirectToAction(nameof(Index));
            }

            if (taVM.formFile != null) // Replace
            {
                if (existing.Image != null)
                {
                    result = ImageService.DeleteImage(existing.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(taVM);
                    }
                }

                result = ImageService.UploadNewImage(taVM.formFile);
                if (result.Success)
                    taVM.ItemVariant.Image = result.Image;
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                    return View(taVM);
                }
            }
            else
            {
                if (taVM.deleteImage)
                {
                    if (existing.Image != null)
                    {
                        result = ImageService.DeleteImage(existing.Image);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(taVM);
                        }
                    }
                }
                else
                {
                    taVM.ItemVariant.Image = existing.Image;
                }
            }

            _UnitOfWork.TargetAudiences.DetachEntity(existing);
            var updateResult = await _UnitOfWork.TargetAudiences.UpdateAsync(taVM.ItemVariant);
            if (updateResult)
            {
                TempData["Success"] = "Target audience updated successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error updating target audience";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Policy = "TargetAudience.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ta = await _UnitOfWork.TargetAudiences.GetOneAsync(t => t.Id == id);
            if (ta is null)
            {
                TempData["Error"] = "Target audience not found";
                return RedirectToAction(nameof(Index));
            }

            if (ta.Image != null)
            {
                var result = ImageService.DeleteImage(ta.Image);
                if (!result.Success)
                {
                    TempData["Error"] = result.ErrorMessage;
                    return RedirectToAction(nameof(Index));
                }
            }

            var deleteResult = await _UnitOfWork.TargetAudiences.DeleteAsync(ta);
            if (deleteResult)
            {
                TempData["Success"] = "Target audience deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error deleting target audience";
            return RedirectToAction(nameof(Index));
        }
    }
}