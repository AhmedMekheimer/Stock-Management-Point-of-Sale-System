using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class BranchItemRepository : Repository<BranchItem>, IBranchItemRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
