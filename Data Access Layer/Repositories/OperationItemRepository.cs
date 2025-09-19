using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class OperationItemRepository : Repository<OperationItem>, IOperationItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OperationItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public OperationItem? LastOrDefault(Expression<Func<OperationItem, bool>>? expression = null)
        {
          return expression != null ? _context.OperationItems.LastOrDefault(expression) : _context.OperationItems.LastOrDefault();
        }
    }
}
