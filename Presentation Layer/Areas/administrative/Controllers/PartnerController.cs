using AspNetCoreGeneratedDocument;
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
        public async Task<IActionResult> Index()
        {
        var partners =  await _UnitOfWork.Partners.GetAsync();

            return View(partners);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? Id)
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
             {new SelectListItem{Text = "Supplier" , Value = "1"}},
             {new SelectListItem{Text = "Customer"  , Value = "2"}},
            };

            PartnerVM partnerVM = new PartnerVM() { 
            PartnerList =  list
            };

            if (Id is null)
            {
                View(partnerVM);
            }
            else
            {
                var partner = await _UnitOfWork.Partners.GetOneAsync(x => x.Id == Id);

                if (partner is not null)
                {

                    partnerVM = partner.Adapt<PartnerVM>();
                    partnerVM.PartnerList = list;
                    return View(partnerVM);
                }

            }

                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(PartnerVM partnerVM)
        {


            if(partnerVM.Id is null)
            {

              var partner = partnerVM.Adapt<Partner>();

              var result =  await _UnitOfWork.Partners.CreateAsync(partner);

                if(result)
                {
                    TempData["success"] = "Partner is added.";
                    return RedirectToAction(nameof(Index));
                }
            } else
            {
                var oldPartner = await _UnitOfWork.Partners.GetOneAsync(x => x.Id == partnerVM.Id ,tracked : true);

                if (oldPartner is not null) {

                   oldPartner =  partnerVM.Adapt<Partner>();

                   var result =await  _UnitOfWork.Partners.UpdateAsync(oldPartner);

                    if(result)
                    {
                        TempData["success"] = "Partner is updated.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

                TempData["error"] = "Somthing is wrong";
                return RedirectToAction(nameof(Index));
        }



        [HttpPost]
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
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
