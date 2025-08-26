using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<TargetAudience> TargetAudiences { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<BranchItem> BranchItems { get; set; }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationItem> OperationItems { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<ReceiveOrder> ReceiveOrders { get; set; }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<TaxReceiveOrder> TaxReceiveOrders { get; set; }
        public DbSet<DiscountOperation> DiscountOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Uniqueness of Tax & Discount Names
            modelBuilder.Entity<Tax>(e =>
                 e.HasIndex(x => new { x.Name }).IsUnique());

            modelBuilder.Entity<Discount>(e =>
                 e.HasIndex(x => new { x.Name }).IsUnique());

            // Tax - Receive Order Bridge Table relation configuration
            modelBuilder.Entity<TaxReceiveOrder>()
                .HasKey(to => new { to.TaxId, to.OperationId });  // Composite PK

            modelBuilder.Entity<TaxReceiveOrder>()
                .HasOne(to => to.Tax)
                .WithMany(b => b.TaxReceiveOrders)
                .HasForeignKey(to => to.TaxId);

            modelBuilder.Entity<TaxReceiveOrder>()
                .HasOne(to => to.ReceiveOrder)
                .WithMany(i => i.TaxReceiveOrders)
                .HasForeignKey(to => to.OperationId);

            // Discount - Operation Bridge Table relation configuration
            modelBuilder.Entity<DiscountOperation>()
                .HasKey(to => new { to.DiscountId, to.OperationId });  // Composite PK

            modelBuilder.Entity<DiscountOperation>()
                .HasOne(to => to.Discount)
                .WithMany(b => b.DiscountOperations)
                .HasForeignKey(to => to.DiscountId);

            modelBuilder.Entity<DiscountOperation>()
                .HasOne(to => to.Operation)
                .WithMany(i => i.DiscountOperations)
                .HasForeignKey(to => to.OperationId);

            // Branch - Item Bridge Table relation configuration
            modelBuilder.Entity<BranchItem>()
                .HasKey(bi => new { bi.BranchId, bi.ItemId });  // Composite PK

            modelBuilder.Entity<BranchItem>()
                .HasOne(bi => bi.Branch)
                .WithMany(b => b.BranchItems)
                .HasForeignKey(bi => bi.BranchId);

            modelBuilder.Entity<BranchItem>()
                .HasOne(bi => bi.Item)
                .WithMany(i => i.BranchItems)
                .HasForeignKey(bi => bi.ItemId);

            // Item Table relation with Item Variants Tables
            // Restricted Delete Behavior for any Item Variant if it is used in an Item
            modelBuilder.Entity<Item>()
                .HasOne(i=>i.Brand)
                .WithMany(b => b.Items)
                .HasForeignKey(i=>i.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Color)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemType)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.ItemTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Size)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.TargetAudience)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.TargetAudienceId)
                .OnDelete(DeleteBehavior.Restrict);

            //Uniquness of Item's Name & Barcode
            modelBuilder.Entity<Item>(e =>
            e.HasIndex(x => new { x.Barcode, x.Name }).IsUnique());

            // ItemType referencing (Tree-Structure)
            modelBuilder.Entity<ItemType>(e =>
            {
                // Self reference: ItemTypeId is the ParentId
                e.HasOne(x => x.Parent)
                 .WithMany(x => x.Children)
                 .HasForeignKey(x => x.ItemTypeId)
                 .OnDelete(DeleteBehavior.Restrict); // safer; we delete descendants manually

                // Unique among siblings (only when parent exists)
                e.HasIndex(x => new { x.ItemTypeId, x.Name }).IsUnique();

                // Unique for root nodes (no parent)
                e.HasIndex(x => x.Name)
                 .IsUnique()
                 .HasFilter("[ItemTypeId] IS NULL");
            });

            // Uniqueness of Operation's Reference in a Transaction
            modelBuilder.Entity<Transaction>()
                .HasIndex(u => u.OperationId)
                .IsUnique();

            // Configure Cashiers (One-to-Many)
            modelBuilder.Entity<Branch>()
                .HasMany(b => b.Cashiers)
                .WithOne(u => u.Branch)
                .HasForeignKey(u => u.BranchId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade issues

            // Configure Branch Manager (One-to-One)
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.BranchManager)
                .WithOne(u => u.ManagedBranch)
                .HasForeignKey<Branch>(b => b.BranchManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TPT inheritance
            modelBuilder.Entity<Operation>().ToTable("Operations");
            modelBuilder.Entity<Transfer>().ToTable("Transfers");
            modelBuilder.Entity<SalesInvoice>().ToTable("SalesInvoices");
            modelBuilder.Entity<ReceiveOrder>().ToTable("ReceiveOrders");

            // OperationItem relationship
            modelBuilder.Entity<OperationItem>()
                .HasOne(oi => oi.Operation)
                .WithMany(o => o.OperationItems)
                .HasForeignKey(oi => oi.OperationId);

            modelBuilder.Entity<OperationItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OperationItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transfer relationships
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.FromBranch)
                .WithMany(b => b.OutgoingTransfers)
                .HasForeignKey(t => t.FromBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.ToBranch)
                .WithMany(b => b.IncomingTransfers)
                .HasForeignKey(t => t.ToBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // SalesInvoice relationships
            modelBuilder.Entity<SalesInvoice>()
                .HasOne(si => si.Branch)
                .WithMany(b => b.SalesInvoices)
                .HasForeignKey(si => si.BranchId);

            modelBuilder.Entity<SalesInvoice>()
                .HasOne(si => si.RetailCustomer)
                .WithMany(p => p.SalesInvoices)
                .HasForeignKey(si => si.RetailCustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReceiveOrder relationships
            modelBuilder.Entity<ReceiveOrder>()
                .HasOne(ro => ro.Branch)
                .WithMany(b => b.ReceiveOrders)
                .HasForeignKey(ro => ro.BranchId);

            modelBuilder.Entity<ReceiveOrder>()
                .HasOne(ro => ro.Supplier)
                .WithMany(p => p.ReceiveOrders)
                .HasForeignKey(ro => ro.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);                
        }
    }
}
