using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize(Policy = SD.Managers)]
    public class TargetAudienceController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;

        public TargetAudienceController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var TargetAudiencesList = await _UnitOfWork.TargetAudiences.GetAsync();
            return View(TargetAudiencesList);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int id = 0)
        {
            var TargetAudienceVM = new TargetAudience();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.TargetAudiences.GetOneAsync(b => b.Id == id)) is TargetAudience targetAudience)
                {
                    TargetAudienceVM = targetAudience;
                    return View(TargetAudienceVM);
                }

                TempData["Error"] = "Target Audience Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(TargetAudienceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(TargetAudience TargetAudienceVM)
        {
            // Saving a Newly-Added Target Audience
            if (TargetAudienceVM.Id == 0)
            {
                var createResult = await _UnitOfWork.TargetAudiences.CreateAsync(TargetAudienceVM);
                if (createResult)
                {
                    TempData["Success"] = "Target Audience Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Target Audience";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Target Audience
            var updateResult = await _UnitOfWork.TargetAudiences.UpdateAsync(TargetAudienceVM);
            if (updateResult)
            {
                TempData["Success"] = "Target Audience Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Error Updating Target Audience";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.TargetAudiences.GetOneAsync(b => b.Id == id)) is TargetAudience targetAudience)
            {
                var deleteResult = await _UnitOfWork.TargetAudiences.DeleteAsync(targetAudience);
                if (deleteResult)
                {
                    TempData["Success"] = "Target Audience Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Target Audience";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Target Audience Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}