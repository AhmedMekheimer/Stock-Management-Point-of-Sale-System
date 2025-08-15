﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces.IRepositories
{
    public interface IRepository<T> where T : class
    {
        // CRUD
        Task<bool> CreateAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);

        Task<List<T>> GetAsync(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true);

        Task<T?> GetOneAsync(Expression<Func<T, bool>>? condition = null, List<Func<IQueryable<T>, IQueryable<T>>>? includes = null, bool tracked = true);

        Task<bool> CommitAsync();
        void DetachEntity(T entity);
    }
}
