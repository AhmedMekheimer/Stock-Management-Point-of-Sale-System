﻿using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using Infrastructure_Layer.Data;
using InfrastructureLayer.Interfaces.IRepositories;
using InfrastructureLayer.Interfaces.IRepositories.ItemVarients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class TargetAudienceRepository : Repository<TargetAudience>, ITargetAudienceRepository
    {
        private readonly ApplicationDbContext _context;
        public TargetAudienceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
