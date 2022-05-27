using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISalaryRepository
    {
        Task<Salary?> GetSalaryByUserIdAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryAsync();
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryAsync(Salary salary);
        Task SaveChangesAsync();
    }
}
