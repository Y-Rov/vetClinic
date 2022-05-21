using Core.Entities;

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
