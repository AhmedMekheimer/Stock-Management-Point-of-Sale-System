﻿using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
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
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
