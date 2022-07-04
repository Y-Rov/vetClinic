using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;
        private readonly IUserSpecializationRepository _usrerSpecializationRepository;

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
