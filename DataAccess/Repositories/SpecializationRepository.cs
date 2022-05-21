using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        ClinicContext _context;
        public SpecializationRepository(ClinicContext context)
        {
            _context = context;
        }
        public async Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSpecializationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Specialization> UpdateSpecializationAsync(int id, Specialization updatedSpecialization)
        {
            throw new NotImplementedException();
        }
    }
}
