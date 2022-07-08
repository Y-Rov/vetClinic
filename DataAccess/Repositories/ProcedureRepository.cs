using System.Linq.Expressions;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProcedureRepository : Repository<Procedure>, IProcedureRepository
{
    private readonly ClinicContext _clinicContext;
    public ProcedureRepository(ClinicContext clinicContext) : base(clinicContext)
    {
        _clinicContext = clinicContext;
    }

    public async Task<PagedList<Procedure>> GetPaged(       
        ProcedureParameters parameters,
        Expression<Func<Procedure, bool>>? filter = null,
        Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>? orderBy = null,
        string includeProperties = "")
    {
        var procedures = await GetQuery(            
            filter: filter,
            orderBy: orderBy,
            orderByDirection: parameters.OrderByDirection,
            includeProperties: includeProperties).ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        return procedures;
    }
    
    private IQueryable<Procedure> GetQuery(
        Expression<Func<Procedure, bool>>? filter = null,
        Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>? orderBy = null,
        string orderByDirection = "",
        string includeProperties = "")
    {
        IQueryable<Procedure> procedureQuery = _clinicContext.Procedures;
            //(await GetAsync(includeProperties: includeProperties)).AsQueryable();

        if (!string.IsNullOrEmpty(includeProperties))
        {
            procedureQuery = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(procedureQuery, (current, includeProperty)
                    => current.Include(includeProperty));
        }    
            
        if (filter is not null)
        {
            procedureQuery = procedureQuery.Where(filter);
        }

        if (orderBy is not null)
        {
            procedureQuery = orderBy(procedureQuery);
            if (!string.IsNullOrEmpty(orderByDirection) && orderByDirection == "desc")
                procedureQuery = procedureQuery.Reverse();
        }

        return procedureQuery;
    }
}