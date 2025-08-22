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
        //var partnerVM =  partners.Adapt<PartnerVM>();

            return View(partners);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? Id)
        {
            List<SelectListItem> list = new List<SelectListItem>() { 
             {new SelectListItem{Text = "Supplier" , Value = "1"}},
             {new SelectListItem{Text = "CorporateCustomer"  , Value = "2"}},
             {new SelectListItem{Text = "Retail"  , Value = "3"}},
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
        public IActionResult Save(PartnerVM partnerVM)
        {


            return View();
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
