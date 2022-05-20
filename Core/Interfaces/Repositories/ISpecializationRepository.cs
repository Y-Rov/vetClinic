using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        Task<ICollection<Specialization>> GetAllSpecializations();
        Task<Specialization> GetSpecializationById(int id);
        Task<int> DeleteSpecialization(int id);
        Task<Specialization> AddSpecialization(Specialization specialization);
        Task<Specialization> UpdateSpecialization(int id, Specialization updatedSpecialization);
    }
}
