using CoreLayer.Models;

namespace PresentationLayer.Areas.DashBoard.ViewModels
{
    public class DashboardVM
    {
        public int NumOfBranches { get; set; }
        public int NewBranchesYearly { get; set; }
        public int NumOfStockItems { get; set; }
        public int NewItemsMonthly { get; set; }

        public int SalesOfMonth { get; set; }
        public decimal MonthlySalesRate { get; set; }
        public int SalesOfToday { get; set; }
        public decimal DailySalesRate { get; set; }

        public decimal AvgInvValMonth { get; set; }
        public decimal AvgInvValRate { get; set; }

        public decimal AvgSalesPerDay { get; set; }
        public decimal AvgSalesPerDayRate { get; set; }

        public decimal TotalStockVal { get; set; }

        // List for Charts
        public List<string> Last12MonthsLabels { get; set; } = new List<string>();
        public List<int> Last12MonthsSales { get; set; } = new List<int>();
        public List<int> Last12MonthsPurchases { get; set; } = new List<int>();

        public List<BranchSalesSummary> TopSellingBranches { get; set; } = new List<BranchSalesSummary>();
        public List<ItemQuantitySummary> TopSellingItems { get; set; } = new List<ItemQuantitySummary>();
    }
}
