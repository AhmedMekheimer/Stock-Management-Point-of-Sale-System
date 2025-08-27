namespace PresentationLayer.Areas.Branch.ViewModels
{
    public class BranchItemDTO
    {
        public int ItemId { get; set; }
        public int BranchId{ get; set; }
        public int Quantity { get; set; }
        public double BuyingPriceAvg { get; set; }
        public double LastBuyingPrice { get; set; }
        public double SellingPrice { get; set; }

    }
}
