using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        readonly ClinicContext context;

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

        public async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            var result = GetQuery(
                filter: filter,
                includeProperties: includeProperties,
                orderBy: null);

            if (asNoTracking)
                return await result
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

            return await result.FirstOrDefaultAsync();
        }

        public async Task<T?> GetById(int id, string includeProperties = "")
        {
            if(string.IsNullOrEmpty(includeProperties))
                return await context.Set<T>().FindAsync(id);

            var result = await context.Set<T>().FindAsync(id);

            IQueryable<T> set = context.Set<T>();

            set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            return await set.FirstOrDefaultAsync(entity => entity == result);
        }

        public void Delete(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                context.Attach(entity);

            context.Entry(entity).State = EntityState.Deleted;
        }

        public void Update(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
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
