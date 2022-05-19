using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISpecializationService
    {
        Task<IEnumerable<Specialization>> GetAllSpecializations();
        Task<Specialization> GetSpecializationById(int id);
        Task<Specialization> AddSpecialization(Specialization specialization);
        Task<Specialization> UpdateSpecialization(int id, Specialization updated);
        Task<int> DeleteSpecialization(int id);
    }
}
