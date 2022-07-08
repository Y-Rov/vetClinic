using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface ISpecializationRepository : IRepository<Specialization>
    { 
        Task UpdateProceduresAsync(int specializationId, IEnumerable<int> procedureIds);
        Task UpdateUsersAsync(int specializationId, IEnumerable<int> userIds);
        Task<PagedList<Specialization>> GetAllAsync(
            SpecializationParameters parameters,
            Expression<Func<Specialization, bool>>? filter = null,
            Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>? orderBy = null,
            Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>? includeProperties = null);
    }
}
