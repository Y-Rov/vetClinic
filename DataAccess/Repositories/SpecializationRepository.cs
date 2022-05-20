using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        ClinicContext _context;
        public SpecializationRepository(ClinicContext context)
        {
            _context = context;
        }
        public Task<Specialization> AddSpecializationAsync(Specialization specialization)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSpecializationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Specialization>> GetAllSpecializationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> GetSpecializationByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> UpdateSpecializationAsync(int id, Specialization updatedSpecialization)
        {
            throw new NotImplementedException();
        }
    }
}
