using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly ApplicationDbContext _context;
        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
