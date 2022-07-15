using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private IQueryable<Specialization> GetQuery(
           Expression<Func<Specialization, bool>>? filter = null,
           Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>? orderBy = null,
           Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>? includeProperties = null,
           bool asNoTracking = false)
        {
            IQueryable<Specialization> specializationsQuery = (
                filter is null
                ? DbSet
                : DbSet.Where(filter)
            ); 

            if (includeProperties is not null)
                specializationsQuery = includeProperties(specializationsQuery);

            if (orderBy is not null)
                specializationsQuery = orderBy(specializationsQuery);

            if (asNoTracking)
                specializationsQuery = specializationsQuery.AsNoTracking();

            return specializationsQuery;
        }

        public SpecializationRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public async Task<PagedList<Specialization>> GetAllAsync(
            SpecializationParameters parameters, 
            Expression<Func<Specialization, bool>>? filter = null,
            Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>? orderBy = null,
            Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>? includeProperties = null)
        {
            var specializations = await GetQuery(
                filter, 
                orderBy, 
                includeProperties,
                asNoTracking: true)
                .ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            return specializations;
        }
    }
}
