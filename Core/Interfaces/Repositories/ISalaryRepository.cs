using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface ISalaryRepository: IRepository<Salary>
    {
        Task<PagedList<Salary>> GetAsync(
            SalaryParametrs parametrs,
            Expression<Func<Salary, bool>>? filter = null);

        Task<Salary?> GetByIdForStatement(int id, Expression<Func<Salary, bool>> filter);
        Task<IEnumerable<Salary>> GetAllForStatement(Expression<Func<Salary, bool>> filter);
    }
}
