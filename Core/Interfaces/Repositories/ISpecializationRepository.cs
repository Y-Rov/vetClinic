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
        Task<ICollection<Specialization>> GetAllSpecializationsAsync();
        Task<Specialization> GetSpecializationByIdAsync(int id);
        Task<int> DeleteSpecializationAsync(int id);
        Task<Specialization> AddSpecializationAsync(Specialization specialization);
        Task<Specialization> UpdateSpecializationAsync(int id, Specialization updatedSpecialization);
    }
}
