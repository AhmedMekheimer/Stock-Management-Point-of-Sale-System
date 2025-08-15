﻿using CoreLayer.Models;
using Infrastructure_Layer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
