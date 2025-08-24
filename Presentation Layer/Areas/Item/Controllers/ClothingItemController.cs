using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.Stock.Controllers
{
    [Area("Item")]
    public class ClothingItemController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ClothingItemController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var itemsList = await _UnitOfWork.Items.GetAsync(include: [i =>i.Color ,
                i => i.TargetAudience , i => i.Brand , i => i.Size , i =>i.ItemType]);
            return View(itemsList);
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            var itemVM = new ItemVM();

            if (id is not null)
            {
                var item = await _UnitOfWork.Items.GetOneAsync(i => i.Id == id);
                itemVM = item.Adapt<ItemVM>();
            }
            LoadData(itemVM).GetAwaiter().GetResult();

            return View(itemVM);
        }




        [HttpPost]
        public async Task<IActionResult> Save(ItemVM itemVM)
        {
            LoadData(itemVM).GetAwaiter().GetResult();
            if (_UnitOfWork.Items.IsNameExist(itemVM.Name, itemVM.Id))
            {
                TempData["error"] = "Name is already exist";
                return View(itemVM);
            }
            if (_UnitOfWork.Items.IsBarcodeExist(itemVM.Barcode, itemVM.Id))
            {
                TempData["error"] = "Barcode is already exist";
                return View(itemVM);
            }

            if (itemVM.Id != 0)
            {

                var item = await _UnitOfWork.Items.GetOneAsync(i => i.Id == itemVM.Id, tracked: true);
                item = itemVM.Adapt<CoreLayer.Models.Item>();

                var result = await _UnitOfWork.Items.UpdateAsync(item);

                if (result)
                {
                    TempData["success"] = "Item Updated";
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                var item = itemVM.Adapt<CoreLayer.Models.Item>();

                var result = await _UnitOfWork.Items.CreateAsync(item);

                if (result)
                {
                    TempData["success"] = "Item Added";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "Somthing went wrong!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Items.GetOneAsync(b => b.Id == id)) is CoreLayer.Models.Item Item)
            {
                var deleteResult = await _UnitOfWork.Items.DeleteAsync(Item);
                if (deleteResult)
                {
                    TempData["Success"] = "Item Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Item";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Item Not Found";
            return RedirectToAction(nameof(Index));
        }

        public async Task LoadData(ItemVM itemVM)
        {

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
    }
}
