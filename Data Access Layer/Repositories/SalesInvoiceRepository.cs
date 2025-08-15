using CoreLayer.Models;
using CoreLayer.Models.Operations;
using Infrastructure_Layer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class SalesInvoiceRepository : Repository<SalesInvoice>, ISalesInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesInvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
