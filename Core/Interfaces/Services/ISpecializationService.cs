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
        Task<IEnumerable<Specialization>> GetAllSpecializationsAsync();
        Task<Specialization> GetSpecializationByIdAsync(int id);
        Task<Specialization> AddSpecializationAsync(Specialization specialization);
        Task<Specialization> UpdateSpecializationAsync(int id, Specialization updated);
        Task<int> DeleteSpecializationAsync(int id);
    }
}
