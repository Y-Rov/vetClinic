using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        readonly ClinicContext context;

        PropertyInfo GetIdProperty()
        {
            var type = typeof(T);

            var properties = type.GetProperties();
            //context.Find(id)

            return type.GetProperties()
                .FirstOrDefault(property => property.Name.Contains("Id")
                    && property.PropertyType == typeof(int));
        }

        public Repository(ClinicContext context)
        {
            this.context = context;
        }

        public IQueryable<T> GetQuery(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string includeProperties = "")
        {
            IQueryable<T> set = filter == null ? context.Set<T>() :
                 context.Set<T>().Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
                set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));

            if (orderBy != null)
                set = orderBy(set);

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
                return await query.AsNoTracking().ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            return await GetQuery(filter: filter, 
                includeProperties: includeProperties, 
                asNoTracking: asNoTracking)
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetById(int id)
        {
            var desiredProperty = GetIdProperty();
            return GetQuery().AsEnumerable()
                .FirstOrDefault(entity => (int)desiredProperty.GetValue(entity) == id);
        }

        public void Delete(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                context.Attach(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }
        public void Update(T entity)
        {
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public async Task InsertAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
