using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        public Task<Specialization> AddSpecialization(Specialization specialization)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteSpecialization(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> GetSpecializationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Specialization> UpdateSpecialization(int id, Specialization updated)
        {
            throw new NotImplementedException();
        }
    }
}
