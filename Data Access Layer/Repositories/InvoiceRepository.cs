using CoreLayer.Models;
using CoreLayer.Models.Operations;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
