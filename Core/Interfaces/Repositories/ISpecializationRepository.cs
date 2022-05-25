using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<Specialization>> GetAllSpecializationsAsync();
        Task<Specialization> GetSpecializationByIdAsync(int id);
        Task<Specialization> AddSpecializationAsync(Specialization specialization);
        Task DeleteSpecializationAsync(Specialization id);
        Task SaveChangesAsync();
    }
}
