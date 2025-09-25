using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PresentationLayer.Areas.DashBoard.ViewModels;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;
        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }

        public async Task<IActionResult> Index(DashboardVM vm)
        {
            vm.NumOfBranches = await _UnitOfWork.Branches.CountAsync();
            vm.NewBranchesYearly = await _UnitOfWork.Branches.CountAsync(b => b.CreatedDate.Year == DateTime.Now.Year);

            vm.NumOfUsers = await _UserManager.Users.CountAsync();
            vm.NewUsersMonthly = await _UserManager.Users.CountAsync(u => u.CreatedDate.Month == DateTime.Now.Month);

            vm.NumOfStockItems = await _UnitOfWork.Items.CountAsync();
            vm.NewItemsMonthly = await _UnitOfWork.Items.CountAsync(i => i.CreatedDate.Month == DateTime.Now.Month);

            decimal lastMonthSales = await _UnitOfWork.SalesInvoices.SumAsync(s => (decimal)s.RoundedGrandTotal, s => s.Date.Month == DateTime.Now.Month - 1);
            vm.SalesOfMonth = (int)await _UnitOfWork.SalesInvoices.SumAsync(s => (decimal)s.RoundedGrandTotal, s => s.Date.Month == DateTime.Now.Month);
            if (lastMonthSales == 0)
            {
                vm.MonthlySalesRate = 0;
            }
            else
            {
                vm.MonthlySalesRate = (((decimal)vm.SalesOfMonth - lastMonthSales) / lastMonthSales) * 100;
            }

            // Today's Sales + Compared to yesterday's Sales
            decimal yesterdaySales = await _UnitOfWork.SalesInvoices.SumAsync(s => (decimal)s.RoundedGrandTotal, s => s.Date.Day == DateTime.Now.Day - 1);
            vm.SalesOfToday = (int)await _UnitOfWork.SalesInvoices.SumAsync(s => (decimal)s.RoundedGrandTotal, s => s.Date.Day == DateTime.Now.Day);
            if (yesterdaySales == 0)
            {
                vm.DailySalesRate = 0;
            }
            else
            {
                vm.DailySalesRate = (((decimal)vm.SalesOfToday - yesterdaySales) / (decimal)yesterdaySales) * 100;
            }

            // Average Invoice Value This Month + Rate Compared to Last Month
            int CurrNumOfInv = await _UnitOfWork.SalesInvoices.CountAsync(s => s.Date.Month == DateTime.Now.Month);
            int LastNumOfInv = await _UnitOfWork.SalesInvoices.CountAsync(s => s.Date.Month == DateTime.Now.Month - 1);
            decimal AvgInvValLastMonth;
            if (CurrNumOfInv > 0)
            {
                vm.AvgInvValMonth = (await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal, s => s.Date.Month == DateTime.Now.Month)) / CurrNumOfInv;
                if (LastNumOfInv > 0)
                {
                    AvgInvValLastMonth = (await _UnitOfWork.SalesInvoices.SumAsync(s => s.RoundedGrandTotal, s => s.Date.Month == DateTime.Now.Month - 1)) / LastNumOfInv;
                    vm.AvgInvValRate = (((decimal)vm.AvgInvValMonth - AvgInvValLastMonth) / (decimal)AvgInvValLastMonth) * 100;
                }
                else
                {
                    vm.AvgInvValStatus = -2; // For No Invoices Last month
                    vm.AvgInvValRate = 0;
                }
            }
            else
            {
                vm.AvgInvValStatus = -1; // For No Invoices This month yet
                vm.AvgInvValRate = 0;
            }


            return View(vm);
        }
    }
}
