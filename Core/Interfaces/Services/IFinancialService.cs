using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IFinancialService
    {
        //public void GenerateFinancialStatementForMonth();
        //public void GenerateFinancialStatementForHalfOfYear();
        //public void GenerateFinancialStatementForYear();
        Task<Salary> GetSalaryByUserIdAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryAsync();
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryByUserIdAsync(int id);
        Task CleanOldSalariesAsync();
        Task<IEnumerable<User>> GetEmployeesWithoutSalary();
    }
}
