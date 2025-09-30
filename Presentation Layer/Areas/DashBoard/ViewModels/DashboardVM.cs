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
    }
}
