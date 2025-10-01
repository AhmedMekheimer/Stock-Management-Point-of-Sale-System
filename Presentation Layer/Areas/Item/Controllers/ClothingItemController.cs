using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Branch.ViewModels;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;
using PresentationLayer.Utility;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.Stock.Controllers
{
    [Area("Item")]
    [Authorize]
    public class ClothingItemController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ClothingItemController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [Authorize(Policy = "ClothingItem.View")]
        public async Task<IActionResult> Index(ItemsWithFiltersVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            // Fetching with Search & Filters
            var items = await _UnitOfWork.Items.GetAsync(s =>
                (string.IsNullOrEmpty(vm.Search)
                || s.Name.Contains(vm.Search))
                && (vm.BrandId == 0 || s.BrandId == vm.BrandId)
                && (vm.ColorId == 0 || s.ColorId == vm.ColorId)
                && (vm.SizeId == 0 || s.SizeId == vm.SizeId)
                && (vm.TargetAudienceId == 0 || s.TargetAudienceId == vm.TargetAudienceId)
                && (vm.ItemTypeId == 0 || s.ItemTypeId == vm.ItemTypeId),
                include: 
                [i =>i.Color, i => i.TargetAudience, i => i.Brand, i => i.Size, i =>i.ItemType], false);

            vm.Brands = (await _UnitOfWork.Brands.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });
            vm.Colors = (await _UnitOfWork.Colors.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });
            vm.Sizes = (await _UnitOfWork.Sizes.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });
            vm.TargetAudiences = (await _UnitOfWork.TargetAudiences.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });
            vm.ItemTypes = (await _UnitOfWork.ItemTypes.GetLeafNodesAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });

            int totalPages = 0;
            if (items.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(items.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.Items = items.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "ClothingItem.Add|ClothingItem.Edit")]
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
                    LoadData(itemVM).GetAwaiter().GetResult();
                    ViewBag.ShowBranchItems = true;
                    var branchItems = await _UnitOfWork.BranchItems.GetAsync(b => b.ItemId == id, include: [b => b.Branch]);
                    itemVM.BranchItem = branchItems;
                    return View(itemVM);
                }
                TempData["Error"] = "Clothing Item Not Found";
                return RedirectToAction(nameof(Index));
            }
            // Display Add Page
            ViewBag.ShowBranchItems = false;
            LoadData(itemVM).GetAwaiter().GetResult();
            return View(itemVM);
        }


        [HttpPost]
        [Authorize(Policy = "ClothingItem.BranchItem")]
        public async Task<IActionResult> SaveBranchItem(BranchItemDTO branchItemDTO)
        {
            var branchItem = await _UnitOfWork.BranchItems.GetOneAsync(b => b.BranchId == branchItemDTO.BranchId && b.ItemId == branchItemDTO.ItemId);

            if (branchItem is null)
                return Json(new { status = false });

            branchItem.BuyingPriceAvg = branchItemDTO.BuyingPriceAvg;
            branchItem.LastBuyingPrice = branchItemDTO.LastBuyingPrice;
            branchItem.SellingPrice = branchItemDTO.SellingPrice;
            branchItem.Quantity = branchItemDTO.Quantity;
            branchItem.DiscountRate = branchItemDTO.DiscountRate;
            branchItem.RestockThreshold= branchItemDTO.RestockThreshold;
            branchItem.OutDatedInMonths= branchItemDTO.OutDatedInMonths;

            var result = await _UnitOfWork.BranchItems.UpdateAsync(branchItem);

            if (result)
                return Json(new { status = true });
            else
                return Json(new { status = false });

        }


        [HttpPost]
        [Authorize(Policy = "ClothingItem.Add|ClothingItem.Edit")]
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
                    // Checking Name & Barcode Uniqueness
                    if ((await _UnitOfWork.Items.GetOneAsync(e => e.Name == itemVM.Name && e.Id != itemVM.Id) is CoreLayer.Models.Item))
                    {

                        ModelState.AddModelError(nameof(itemVM.Name), "Name already exists");
                        if ((await _UnitOfWork.Items.GetOneAsync(e => e.Barcode == itemVM.Barcode && e.Id != itemVM.Id) is CoreLayer.Models.Item))
                        {
                            ModelState.AddModelError(nameof(itemVM.Barcode), "Barcode already exists");
                        }
                        ViewBag.ShowBranchItems = true;
                        var branchItems = await _UnitOfWork.BranchItems.GetAsync(b => b.ItemId == itemVM.Id, include: [b => b.Branch]);
                        itemVM.BranchItem = branchItems;
                        itemVM.Image = item.Image;
                        return View(itemVM);
                    }
                    else if ((await _UnitOfWork.Items.GetOneAsync(e => e.Barcode == itemVM.Barcode && e.Id != itemVM.Id) is CoreLayer.Models.Item))
                    {
                        ModelState.AddModelError(nameof(itemVM.Barcode), "Barcode already exists");
                        ViewBag.ShowBranchItems = true;
                        var branchItems = await _UnitOfWork.BranchItems.GetAsync(b => b.ItemId == itemVM.Id, include: [b => b.Branch]);
                        itemVM.BranchItem = branchItems;
                        itemVM.Image = item.Image;
                        return View(itemVM);
                    }

                    var newItem = new CoreLayer.Models.Item();
                    newItem = itemVM.Adapt<CoreLayer.Models.Item>();

                    // Putting New Image
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
                        else
                            // Keeping Old Image
                            newItem.Image = item.Image;
                    }
                    _UnitOfWork.Items.DetachEntity(item);

                    var result = await _UnitOfWork.Items.UpdateAsync(newItem);

                    if (result)
                    {
                        TempData["success"] = "Item Updated Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["Error"] = "A Db Error Updating Item";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Clothing Item Not Found";
                return View(itemVM);
            }

            // Saving a New Clothing Item
            else
            {
                // Checking Name & Barcode Uniqueness
                if ((await _UnitOfWork.Items.GetOneAsync(e => e.Name == itemVM.Name) is CoreLayer.Models.Item))
                {
                    ModelState.AddModelError(nameof(itemVM.Name), "Name already exists");
                    if ((await _UnitOfWork.Items.GetOneAsync(e => e.Barcode == itemVM.Barcode) is CoreLayer.Models.Item))
                    {
                        ModelState.AddModelError(nameof(itemVM.Barcode), "Barcode already exists");
                    }
                    ViewBag.ShowBranchItems = false;
                    LoadData(itemVM).GetAwaiter().GetResult();
                    return View(itemVM);
                }
                else if ((await _UnitOfWork.Items.GetOneAsync(e => e.Barcode == itemVM.Barcode) is CoreLayer.Models.Item))
                {
                    ModelState.AddModelError(nameof(itemVM.Barcode), "Barcode already exists");
                    ViewBag.ShowBranchItems = false;
                    LoadData(itemVM).GetAwaiter().GetResult();
                    return View(itemVM);
                }

                var item = itemVM.Adapt<CoreLayer.Models.Item>();

                if (itemVM.formFile != null)
                {
                    // Saving Physically
                    resultImg = ImageService.UploadNewImage(itemVM.formFile);
                    if (resultImg.Success)
                        item.Image = resultImg.Image;
                    else
                    {
                        TempData["Error"] = resultImg.ErrorMessage;
                        return View(itemVM);
                    }
                }

                var result = await _UnitOfWork.Items.CreateAsync(item);

                if (result)
                {
                    var branches = await _UnitOfWork.Branches.GetAsync();
                    var branchItems = new List<BranchItem>();


                    foreach (var itemBranch in branches)
                    {
                        var branchItem = new BranchItem()
                        {
                            Quantity = 0,
                            SellingPrice = 0,
                            BuyingPriceAvg = 0,
                            LastBuyingPrice = 0,
                            ItemId = item.Id,
                            BranchId = itemBranch.Id,
                            DiscountRate = 0,
                        };
                        branchItems.Add(branchItem);
                    }

                    var resultAddingBranchItem = await _UnitOfWork.BranchItems.CreateRangeAsync(branchItems);

                    if (resultAddingBranchItem)
                    {
                        TempData["success"] = "Item Added";
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["Error"] = "A Db Error Adding Item";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [Authorize(Policy = "ClothingItem.Delete")]
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


        [NonAction]
        public async Task LoadData(ItemVM itemVM)
        {

            var brands = await _UnitOfWork.Brands.GetAsync();
            var colors = await _UnitOfWork.Colors.GetAsync();
            var itemTypes = await _UnitOfWork.ItemTypes.GetAsync();
            var sizes = await _UnitOfWork.Sizes.GetAsync();
            var targetAudiences = await _UnitOfWork.TargetAudiences.GetAsync();
            var itemTypesId = _UnitOfWork.ItemTypes.GetAsync().GetAwaiter().GetResult().Select(i => i.ItemTypeId ?? 0).ToList();

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


            itemVM.ItemTypesList = itemTypes.Where(i => !itemTypesId.Contains(i.Id))
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
