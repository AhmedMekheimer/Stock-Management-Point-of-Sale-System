using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;
using PresentationLayer.Utility;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.Stock.Controllers
{
    [Area("Item")]
    [Authorize(Policy = SD.Managers)]
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
        public async Task<IActionResult> Save(int? id = 0)
        {
            var itemVM = new ItemVM();

            // Display Edit Page
            if (id != 0)
            {
                var item = await _UnitOfWork.Items.GetOneAsync(i => i.Id == id);
                if (item != null)
                {
                    itemVM = item.Adapt<ItemVM>();
                    //itemVM.Image = item.Image;
                    LoadData(itemVM).GetAwaiter().GetResult();
                    return View(itemVM);
                }
                TempData["Error"] = "Clothing Item Not Found";
                return RedirectToAction(nameof(Index));
            }
            // Display Add Page
            LoadData(itemVM).GetAwaiter().GetResult();
            return View(itemVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ItemVM itemVM)
        {
            if (!ModelState.IsValid)
                return View(itemVM);
            Result resultImg = new Result();

            LoadData(itemVM).GetAwaiter().GetResult();

            // Saving an Existing Item
            if (itemVM.Id != 0)
            {
                if ((await _UnitOfWork.Items.GetOneAsync(i => i.Id == itemVM.Id) is CoreLayer.Models.Item item))
                    {
                    var newItem = new CoreLayer.Models.Item();
                    newItem=itemVM.Adapt<CoreLayer.Models.Item>();

                    // Replacing with a New Image
                    if (itemVM.formFile != null)
                    {
                        if (item.Image != null)
                        {
                            // Deleting Old Image Physically
                            resultImg = ImageService.DeleteImage(item.Image);
                            if (!resultImg.Success)
                            {
                                TempData["Error"] = resultImg.ErrorMessage;
                                return View(itemVM);
                            }
                        }

                        // Add new Image Physically
                        resultImg = ImageService.UploadNewImage(itemVM.formFile);
                        if (resultImg.Success)
                            newItem.Image = resultImg.Image;
                        else
                        {
                            TempData["Error"] = resultImg.ErrorMessage;
                            return View(itemVM);
                        }
                    }
                    else
                    {
                        // Old Image deleted
                        if (itemVM.deleteImage)
                        {
                            // Deleting Old Image Physically
                            resultImg = ImageService.DeleteImage(item.Image!);
                            if (!resultImg.Success)
                            {
                                TempData["Error"] = resultImg.ErrorMessage;
                                return View(itemVM);
                            }
                        }
                    }
                    _UnitOfWork.Items.DetachEntity(item);

                    var result = await _UnitOfWork.Items.UpdateAsync(newItem);

                    if (result)
                    {
                        TempData["success"] = "Item Updated Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["Error"] = "Error Updating Item";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Clothing Item Not Found";
                return View(itemVM);
            }
            // Saving a New Clothing Item
            else
            {
                var item = itemVM.Adapt<CoreLayer.Models.Item>();

                if (itemVM.formFile != null)
                {
                    // Saving Physically
                    resultImg = ImageService.UploadNewImage(itemVM.formFile);
                    if (resultImg.Success)
                        itemVM.Image = resultImg.Image;
                    else
                    {
                        TempData["Error"] = resultImg.ErrorMessage;
                        return View(itemVM);
                    }
                }

                var result = await _UnitOfWork.Items.CreateAsync(item);

                if (result)
                {
                    TempData["success"] = "Item Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Adding Item";
                return RedirectToAction(nameof(Index));
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var resultImage = new Result();
            if ((await _UnitOfWork.Items.GetOneAsync(b => b.Id == id)) is CoreLayer.Models.Item Item)
            {
                if (Item.Image != null)
                {
                    // Deleting Image Physically
                    resultImage = ImageService.DeleteImage(Item.Image);
                    if (!resultImage.Success)
                    {
                        TempData["Error"] = resultImage.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }

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
