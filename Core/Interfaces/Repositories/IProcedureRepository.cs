using System.Linq.Expressions;
using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository : IRepository<Procedure>
{
    Task<PagedList<Procedure>> GetPaged(
        ProcedureParameters parameters,
        Expression<Func<Procedure, bool>>? filter = null,
        Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>? orderBy = null,
        string includeProperties = "");
}