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
        private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;
        private readonly IUserSpecializationRepository _usrerSpecializationRepository;

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

        public SpecializationRepository(ClinicContext context,
            IProcedureSpecializationRepository procedureSpecializationRepository,
            IUserSpecializationRepository userSpecializationRepository)
            : base(context)
        {
            _procedureSpecializationRepository = procedureSpecializationRepository;
            _usrerSpecializationRepository = userSpecializationRepository;
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

        public async Task UpdateProceduresAsync(int specializationId, IEnumerable<int> proceduresIds)
        {
            var current = await _procedureSpecializationRepository.GetAsync(
                filter: relationship => relationship.SpecializationId == specializationId);

            foreach (var relationship in current)
                _procedureSpecializationRepository.Delete(relationship);

            await _procedureSpecializationRepository.SaveChangesAsync();

            foreach (var procedureId in proceduresIds)
            {
                await _procedureSpecializationRepository.InsertAsync(new ProcedureSpecialization()
                {
                    ProcedureId = procedureId,
                    SpecializationId = specializationId
                });
            }

            await SaveChangesAsync();
        }

        public async Task UpdateUsersAsync(int specializationId, IEnumerable<int> userIds)
        {
            var related = await _usrerSpecializationRepository.GetAsync(
                filter: relationship => relationship.SpecializationId == specializationId);

            foreach (var relationship in related)
                _usrerSpecializationRepository.Delete(relationship);

            await _usrerSpecializationRepository.SaveChangesAsync();

            foreach (var userId in userIds)
                await _usrerSpecializationRepository.InsertAsync(new UserSpecialization
                {
                    UserId = userId,
                    SpecializationId = specializationId
                });

            await SaveChangesAsync();
        }
    }
}
