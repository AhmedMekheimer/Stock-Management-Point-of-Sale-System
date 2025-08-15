using CoreLayer.Models;
using Infrastructure_Layer.Data;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using InfrastructureLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Item Varients Repos
        public IBrandRepository Brands { get; }
        public IColorRepository Colors { get; }
        public IItemTypeRepository ItemTypes { get; }
        public ISizeRepository Sizes { get; }
        public ITargetAudienceRepository TargetAudiences { get; }

        // Operations Repos
        public IOperationRepository Operations { get; }
        public ITransferRepository Transfers { get; }
        public IInvoiceRepository Invoices { get; }
        public ISalesInvoiceRepository SalesInvoices { get; }
        public IReceiveOrderRepository ReceiveOrders { get; }

        public IBranchRepository Branches { get; }
        public IBranchItemRepository BranchItems { get; }
        public IItemRepository Items { get; }
        public IOperationItemRepository OperationItems { get; }
        public IPartnerRepository Partners { get; }
        public ITransactionRepository Transactions { get; }
        public IVoucherRepository Vouchers { get; }
        public IApplicationUserOTPRepository ApplicationUserOTPs { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // Initialize Item Variants Repos
            Brands = new BrandRepository(_context);
            Colors = new ColorRepository(_context);
            ItemTypes = new ItemTypeRepository(_context);
            Sizes = new SizeRepository(_context);
            TargetAudiences = new TargetAudienceRepository(_context);

            // Initialize Operations Repos
            Operations = new OperationRepository(_context);
            Transfers = new TransferRepository(_context);
            Invoices = new InvoiceRepository(_context);
            SalesInvoices = new SalesInvoiceRepository(_context);
            ReceiveOrders = new ReceiveOrderRepository(_context);

            // Initialize Core Repos
            Branches = new BranchRepository(_context);
            BranchItems = new BranchItemRepository(_context);
            Items = new ItemRepository(_context);
            OperationItems = new OperationItemRepository(_context);
            Partners = new PartnerRepository(_context);
            Transactions = new TransactionRepository(_context);
            Vouchers = new VoucherRepository(_context);
            ApplicationUserOTPs=new ApplicationUserOTPRepository(_context);
        }

        public void Dispose() => _context.Dispose();
    }
}
