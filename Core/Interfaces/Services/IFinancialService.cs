using Core.Entities;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IFinancialService
    {
        Task<Salary> GetSalaryByUserIdAsync(int id);
        Task<PagedList<Salary>> GetSalaryAsync(SalaryParametrs paramerts);
        Task CreateSalaryAsync(Salary salary);
        Task UpdateSalaryAsync(Salary salary);
        Task DeleteSalaryByUserIdAsync(int id);
        Task CleanOldSalariesAsync();
        Task<IEnumerable<User>> GetEmployeesWithoutSalary();
        Task<PagedList<FinancialStatement>> GetFinancialStatement(DatePeriod date, FinancialStatementParameters parametrs);
    }
}
