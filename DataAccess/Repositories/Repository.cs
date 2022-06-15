﻿using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ClinicContext _clinicContext;

        public Repository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public IQueryable<T> GetQuery(
            Expression<Func<T, bool>>? filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
            string includeProperties = "")
        {
            IQueryable<T> set = filter == null ? _clinicContext.Set<T>() :
                 _clinicContext.Set<T>().Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                set = orderBy(set);
            }
            
            return set;
        }

        public async Task<IList<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            var query = GetQuery(filter, orderBy, includeProperties);
            
            if (asNoTracking)

            {
                var noTrackingResult = await query.AsNoTracking()
                    .ToListAsync();
                
                return noTrackingResult;
            }

            var trackingResult = await query.ToListAsync();
            return trackingResult;

        }

        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter = null,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            var query = GetQuery(
                filter: filter,
                includeProperties: includeProperties,
                orderBy: null);

            if (asNoTracking)
            {
                var noTrackingResult = await query.AsNoTracking()
                    .FirstOrDefaultAsync();

                return noTrackingResult;
            }
               
            var trackingResult = await query.FirstOrDefaultAsync();
            return trackingResult;
        }

        public async Task<T?> GetById(int id, string includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
            {
                return await _clinicContext.Set<T>().FindAsync(id);
            }
            
            var result = await _clinicContext.Set<T>().FindAsync(id);

            IQueryable<T> set = _clinicContext.Set<T>();

            set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));

            return await set.FirstOrDefaultAsync(entity => entity == result);
        }

        public void Delete(T entity)
        {
            if (_clinicContext.Entry(entity).State == EntityState.Detached)
            {
                _clinicContext.Attach(entity);
            }
            
            _clinicContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            if (_clinicContext.Entry(entity).State == EntityState.Detached)
            {
                _clinicContext.Attach(entity);
            }

            _clinicContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task InsertAsync(T entity)
        {
            await _clinicContext.Set<T>().AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}