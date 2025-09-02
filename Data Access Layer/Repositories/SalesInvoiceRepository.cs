using CoreLayer.Models.Operations;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories.Operations;


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
