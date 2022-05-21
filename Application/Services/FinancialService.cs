using Core.Entities;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class FinancialService:IFinancialService
    {
        public void GenerateFinancialStatementForMonth()
        {
            throw new NotImplementedException();
        }
        public void GenerateFinancialStatementForHalfOfYear()
        {
            throw new NotImplementedException();
        }
        public void GenerateFinancialStatementForYear()
        {
            throw new NotImplementedException();
        }
        public decimal GetSalaryById(int id)
        {
            throw new NotImplementedException();
        }
        public decimal SetSalaryById(int id, decimal value)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Salary> GetSalaries()
        {
            throw new NotImplementedException();
        }
    }
}
