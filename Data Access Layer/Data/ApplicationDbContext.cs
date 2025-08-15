using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Layer.Data
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
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationItem> OperationItems { get; set; }

        // Derived DbSets
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<Invoice> RetailInvoices { get; set; }
        public DbSet<ReceiveOrder> ReceiveOrders { get; set; }

        public DbSet<Partner> Partners { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // ItemType referencing (Tree-Structure)
            modelBuilder.Entity<ItemType>()
            .HasOne(it => it.Parent)
            .WithMany(it => it.Children)
            .HasForeignKey(it => it.ItemTypeId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevents cascade delete loops

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
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<ReceiveOrder>().ToTable("ReceiveOrders");

            // OperationItem relationship
            modelBuilder.Entity<OperationItem>()
                .HasOne(oi => oi.Operation)
                .WithMany(o => o.OperationItems)
                .HasForeignKey(oi => oi.OperationId);

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
                .HasOne(si => si.CorporateCustomer)
                .WithMany(p => p.CorporateSales)
                .HasForeignKey(si => si.CorporateCustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice relationships
            modelBuilder.Entity<Invoice>()
                .HasOne(si => si.Branch)
                .WithMany(b => b.Invoices)
                .HasForeignKey(si => si.BranchId);

            modelBuilder.Entity<Invoice>()
                .HasOne(si => si.RetailCustomer)
                .WithMany(p => p.Invoices)
                .HasForeignKey(si => si.RetailCustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReceiveOrder relationships
            modelBuilder.Entity<ReceiveOrder>()
                .HasOne(ro => ro.Branch)
                .WithMany(b => b.ReceiveOrders)
                .HasForeignKey(ro => ro.BranchId);

            modelBuilder.Entity<ReceiveOrder>()
                .HasOne(ro => ro.Supplier)
                .WithMany(p => p.SupplyOrders)
                .HasForeignKey(ro => ro.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
