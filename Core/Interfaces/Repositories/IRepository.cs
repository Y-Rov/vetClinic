using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<IList<T>> GetAsync(
             Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             string includeProperties = "",
             bool asNoTracking = false);

        Task<T> GetById(int id, string includeProperties = "");

        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "",
            bool asNoTracking = false);

        Task InsertAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task SaveChangesAsync();
    }
}
