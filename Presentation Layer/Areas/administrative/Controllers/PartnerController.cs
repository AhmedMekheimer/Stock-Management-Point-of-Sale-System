using AspNetCoreGeneratedDocument;
using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.administrative.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class PartnerController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;


        public PartnerController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "Partner.View")]
        public async Task<IActionResult> Index()
        {
            var partners = await _UnitOfWork.Partners.GetAsync();

            return View(partners);
        }

        [HttpGet]
        [Authorize(Policy = "Partner.Add|Partner.Edit")]
        public async Task<IActionResult> Save(int Id = 0)
        {
            PartnerVM partnerVM = new PartnerVM();

            if (Id == 0)
            {
                return View(partnerVM);
            }
            else
            {
                var partner = await _UnitOfWork.Partners.GetOneAsync(x => x.Id == Id);

                if (partner is not null)
                {

                    partnerVM = partner.Adapt<PartnerVM>();
                    return View(partnerVM);
                }
                else
                {
                    TempData["error"] = "Partner not found.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = "Partner.Add|Partner.Edit")]
        public async Task<IActionResult> Save(PartnerVM partnerVM)
        {
            if (!ModelState.IsValid)
                return View(partnerVM);

            // Adding New Partner
            if (partnerVM.Id is null)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Partners.GetOneAsync(e => e.Name == partnerVM.Name) is Partner))
                {
                    ModelState.AddModelError(nameof(partnerVM.Name), "Name already exists");
                    return View(partnerVM);
                }
                // Checking Email Uniqueness
                if ((await _UnitOfWork.Partners.GetOneAsync(e => e.Email == partnerVM.Email) is Partner))
                {
                    ModelState.AddModelError(nameof(partnerVM.Email), "Email already exists");
                    return View(partnerVM);
                }
                var partner = partnerVM.Adapt<Partner>();

                var result = await _UnitOfWork.Partners.CreateAsync(partner);

                if (result)
                {
                    TempData["success"] = "Partner Added Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "A Db Error Adding Partner";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                var oldPartner = await _UnitOfWork.Partners.GetOneAsync(x => x.Id == partnerVM.Id, tracked: false);
                if (oldPartner is not null)
                {
                    // Checking Name Uniqueness
                    if ((await _UnitOfWork.Partners.GetOneAsync(e => (e.Name == partnerVM.Name) && e.Id != partnerVM.Id) is Partner))
                    {
                        ModelState.AddModelError(nameof(partnerVM.Name), "Name already exists");
                        return View(partnerVM);
                    }
                    // Checking Email Uniqueness
                    if ((await _UnitOfWork.Partners.GetOneAsync(e => (e.Email == partnerVM.Email) && e.Id != partnerVM.Id) is Partner))
                    {
                        ModelState.AddModelError(nameof(partnerVM.Email), "Email already exists");
                        return View(partnerVM);
                    }
                    oldPartner = partnerVM.Adapt<Partner>();

                    var result = await _UnitOfWork.Partners.UpdateAsync(oldPartner);

                    if (result)
                    {
                        TempData["success"] = "Partner Updated Successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "A Db Error Updating Partner";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["error"] = "Partner not found.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        [HttpPost]
        [Authorize(Policy = "Partner.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _UnitOfWork.Partners.GetOneAsync(x => x.Id == id);

            if (partner is not null)
            {
                var partnerResult = await _UnitOfWork.Partners.DeleteAsync(partner);

                if (partnerResult)
                {
                    TempData["success"] = "Partner Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Db Error Deleting Partner";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["error"] = "Partner not found.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
