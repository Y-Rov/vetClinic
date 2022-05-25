using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISalaryRepository
    {
        Task<Salary?> GetSalaryAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryAsync();
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryAsync(int id);
        Task SaveChangesAsync();
    }
}
