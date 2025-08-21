using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;

namespace PresentationLayer.Areas.Stock.Controllers
{
    [Area("Item")]
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ItemController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var itemsList = await _UnitOfWork.Items.GetAsync();
            return View(itemsList);
        }

/*        // Display Add or Edit Page
        [HttpGet]
        public async Task<IActionResult> Save(string? id)
        {
            // Display Item Varients Select Lists
            var itemVM = new ItemVM();

            var brands = await _UnitOfWork.Brands.GetAsync();
            var colors = await _UnitOfWork.Colors.GetAsync();
            var itemTypes = await _UnitOfWork.ItemTypes.GetAsync();
            var sizes = await _UnitOfWork.Sizes.GetAsync();
            var targetAudiences = await _UnitOfWork.TargetAudiences.GetAsync();

            itemVM.BrandsList = brands
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            itemVM.ColorsList = colors
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            itemVM.ItemTypesList = itemTypes
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            itemVM.SizesList = sizes
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

            itemVM.TargetAudiencesList = targetAudiences
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList();

        }

            // Saving Adding Or Editing a Clothing Item
            [HttpPost]
        public async Task<IActionResult> Save(ItemVM itemVM)
        {

        }*/
    }
}
