namespace Core_Layer
{
    public class SD
    {
        public const string SuperAdmin = "Super Admin";
        public const string Admin = "Admin";
        public const string StockManager = "Stock Manager";
        public const string InventoryManager = "Inventory Manager";
        public const string Cashier = "Cashier";
        public const string Admins = "Admins";
        public const string Workers = "Workers";
        public static readonly List<string> AllRoles = new List<string>()
        {
            SuperAdmin,
            Admin,
            StockManager,
            InventoryManager,
            Cashier
        };
    }
}
