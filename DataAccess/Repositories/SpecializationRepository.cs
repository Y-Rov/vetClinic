using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly IProcedureSpecializationRepository _procedureSpecializationRepository;
        //private readonly IUser
        public SpecializationRepository(ClinicContext context,
            IProcedureSpecializationRepository procedureSpecializationRepository) : base(context)
        {
            _procedureSpecializationRepository = procedureSpecializationRepository;
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

        }
    }
}
