using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using InfrastructureLayer;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.administrative.ViewModels;
using PresentationLayer.Areas.Branch.ViewModels;
using PresentationLayer.Areas.Sales.DTOs;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize]
    public class PosApiController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public PosApiController(IUnitOfWork UnitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = UnitOfWork;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: /api/Sales/PosApi/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            ApplicationUser user = (await _userManager.GetUserAsync(User))!;
            if (user is not null)
            {
                var result =
                    new
                    {
                        id = user.Id,
                        name = user.UserName
                    };
                return Ok(result);
            }
            else
                return NotFound();
        }


        // GET: /api/Sales/PosApi/branches
        [HttpGet("branches")]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                ApplicationUser user = (await _userManager.GetUserAsync(User))!;
                var branches = await _UnitOfWork.Branches.GetAsync();

                if (user.BranchId is not null && branches.Find(b => b.Id == user.BranchId) is CoreLayer.Models.Branch branch)
                {
                    // The user has an assigned branch
                    var result = new[]
                    {
                        new
                        {
                            id = branch.Id,
                            name = branch.Name
                        }
                    };
                    return Ok(result);
                }
                else
                {
                    // Branch to be selected in the page
                    var result = branches.Select(b => new
                    {
                        id = b.Id,
                        name = b.Name
                    }).ToArray();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                // Error Log
                TempData["Error"] = "Error fetching branch data";
                return StatusCode(500, new { message = "An error occurred while fetching branch data" });
            }
        }


        // GET: /api/Sales/PosApi/itemtypes
        [HttpGet("itemtypes")]
        public async Task<IActionResult> GetItemTypes()
        {
            try
            {
                var leafItemTypes = await _UnitOfWork.ItemTypes.GetLeafNodesAsync();
                if (leafItemTypes is null)
                {
                    TempData["Error"] = "Error fetching Item types from Db";
                    return StatusCode(500, new { message = "An error occurred while fetching item types" });
                }
                // Transform the data to match the expected format at pos.js
                var result = leafItemTypes.Select(t => new
                {
                    id = t.Id,
                    name = t.Name
                }).ToArray();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Error Log
                TempData["Error"] = "Error fetching Item types";
                return StatusCode(500, new { message = "An error occurred while fetching item types" });
            }
        }


        // GET: /api/Sales/PosApi/items?typeId?branchId
        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] int typeId, [FromQuery] int branchId)
        {
            try
            {
                ApplicationUser user = (await _userManager.GetUserAsync(User))!;

                // Use Include and ThenInclude to load related data
                var branchItems = await _UnitOfWork.BranchItems.GetAsyncIncludes(
                b => b.BranchId == branchId && b.Quantity > 0,
                new List<Func<IQueryable<BranchItem>, IQueryable<BranchItem>>>
                {
                    b=>b.Include(b => b.Item).ThenInclude(b=>b.ItemType)
                }, false);

                if (branchItems is null)
                {
                    TempData["Error"] = "Error fetching items from Db";
                    return StatusCode(500, new { message = "An error occurred while fetching items" });
                }

                var result = branchItems.Select(i => new
                {
                    id = i.ItemId,
                    name = i.Item.Name,
                    type = i.Item.ItemType.Name,
                    typeId = i.Item.ItemTypeId,
                    barcode = i.Item.Barcode,
                    quantity = i.Quantity,
                    price = i.SellingPrice,
                    discountPrice = (i.DiscountRate is null) ? i.SellingPrice : i.SellingPrice - i.SellingPrice * i.DiscountRate / 100.0,
                    discountRate=i.DiscountRate
                });

                // All Items Button
                if (typeId == 0)
                    return Ok(result.ToArray());
                else
                {
                    result = result.Where(b => b.typeId == typeId);
                    return Ok(result.ToArray());
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching items";
                return StatusCode(500, new { message = "An error occurred while fetching items" });
            }
        }


        // GET: /api/Sales/PosApi/customers
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _UnitOfWork.Partners.GetAsync(p => p.partnerType == Partner.PartnerType.RetailCustomer);

                var result = customers.Select(b => new
                {
                    id = b.Id,
                    name = b.Name
                }).ToArray();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Error Log
                TempData["Error"] = "Error fetching customers";
                return StatusCode(500, new { message = "An error occurred while fetching customers" });
            }
        }


        // GET: /api/Sales/PosApi/discounts
        [HttpGet("discounts")]
        public async Task<IActionResult> GetDiscounts()
        {
            try
            {
                var activeDiscounts = (await _UnitOfWork.Discounts.GetActiveDiscountsAsync()).OrderByDescending(d=>d.Rate).Take(1);
                if (activeDiscounts is null)
                {
                    TempData["Error"] = "Error fetching Discounts from Db";
                    return StatusCode(500, new { message = "An Db error occurred while fetching discounts" });
                }

                // Transform the data to match the expected format at pos.js
                var result = activeDiscounts.Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    rate = t.Rate
                }).ToArray();
                return Ok(result);

            }
            catch (Exception ex)
            {
                // Error Log
                TempData["Error"] = "Error fetching Discounts";
                return StatusCode(500, new { message = "An error occurred while fetching discounts" });
            }
        }


        // POST: /api/Sales/PosApi/createSalesInvoice
        [HttpPost("createSalesInvoice")]
        public async Task<IActionResult> CreateSalesInvoice([FromBody] CreateSalesInvoiceDTO dto)
        {
            if (dto == null)
                return BadRequest("No invoice received");

            var invoice = new SalesInvoice
            {
                BranchId = dto.BranchId,
                RetailCustomerId = dto.RetailCustomerId,
                ApplicationUserId = dto.ApplicationUserId,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow),
                status = Status.Approved,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalDiscountRate = dto.TotalDiscountRate,
                TotalDiscountAmount = dto.TotalDiscountAmount,
                GrandTotal = dto.GrandTotal,
                RoundedGrandTotal = dto.RoundedGrandTotal,
                OperationItems = dto.OperationItems.Select(o => new OperationItem
                {
                    ItemId = o.ItemId,
                    Quantity = o.Quantity,

                    // Original selling price
                    SellingPrice = o.SellingPrice,

                    // Calculate
                    DiscountRate = o.DiscountRate,

                    // final total
                    TotalPrice = o.DiscountPrice * o.Quantity
                }).ToList()
            };

            var createResult = await _UnitOfWork.SalesInvoices.CreateAsync(invoice);
            if(createResult)
            {
                foreach (var item in invoice.OperationItems)
                {
                    item.OperationId = invoice.Id;
                }
                List<DiscountSalesInvoice> discountSalesInvoices = new List<DiscountSalesInvoice>();
                foreach (var discount in dto.GeneralDiscounts)
                {
                    discountSalesInvoices.Add(new DiscountSalesInvoice
                    {
                        DiscountId = discount,
                        OperationId = invoice.Id
                    });
                }
                invoice.DiscountSalesInvoices = discountSalesInvoices;

                var updateResult = await _UnitOfWork.SalesInvoices.UpdateAsync(invoice);
                if(updateResult)
                {
                    // Subtracting from the Stock/Quantity of each item in the branch
                    foreach(var item in invoice.OperationItems)
                    {
                        var branchItem=await _UnitOfWork.BranchItems.GetOneAsync(bi=>bi.BranchId==invoice.BranchId && bi.ItemId==item.ItemId,null,false);
                        if(branchItem is not null)
                        {
                            branchItem.Quantity -= item.Quantity;
                            var lastResult = await _UnitOfWork.BranchItems.UpdateAsync(branchItem);
                            if (lastResult)
                                return Ok();
                        }
                        return BadRequest("Error Subtracting Quantity from Branch");
                    }
                }
                return BadRequest("Error updating Invoice");
            }
            return BadRequest("Error creating Invoice");
        }
    }
}
