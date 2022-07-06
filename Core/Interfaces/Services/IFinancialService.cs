using Core.Entities;
using Core.Models.Finance;
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
        Task<IEnumerable<FinancialStatement>> GetFinancialStatement(DatePeriod date);
    }
}
