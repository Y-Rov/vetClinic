using Core.Entities;
using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IFinancialService
    {
        //public void GenerateFinancialStatementForMonth();
        //public void GenerateFinancialStatementForHalfOfYear();
        //public void GenerateFinancialStatementForYear();
        Task<Salary> GetSalaryByUserIdAsync(int id);
        Task<IEnumerable<Salary>> GetSalaryAsync(Expression<Func<Salary, bool>>? filter);
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryByUserIdAsync(int id);
        Task CleanOldSalariesAsync();
        Task<IEnumerable<User>> GetEmployeesWithoutSalary();
        Task<FinancialStatement> GetFinancialStatement(DateTime startDate);
    }
}
