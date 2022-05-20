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
        public Task<Specialization> AddSpecialization(Specialization specialization)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSpecialization(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Specialization>> GetAllSpecializations()
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> GetSpecializationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> UpdateSpecialization(int id, Specialization updatedSpecialization)
        {
            throw new NotImplementedException();
        }
    }
}
