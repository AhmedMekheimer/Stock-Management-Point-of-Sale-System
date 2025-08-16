using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
