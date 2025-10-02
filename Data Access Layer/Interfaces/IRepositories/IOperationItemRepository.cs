using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IOperationItemRepository : IRepository<OperationItem>
    {
        public OperationItem? LastOrDefault(Expression<Func<OperationItem, bool>>? expression = null);
    }
}
