using Core.Entities;
using Core.Interfaces;

namespace Application.Services
{
    public class FinancialService: IFinancialService
    {
        public void GenerateFinStatementForMonth()
        {
            throw new NotImplementedException();
        }
        public void GenerateFinStatementForHalfOfYear()
        {
            throw new NotImplementedException();
        }
        public void GenerateFinStatementForYear()
        {
            throw new NotImplementedException();
        }
        public void SetSalary(int userId, decimal value)
        {
            throw new NotImplementedException();
        }
        public void ChangeSalary(int userId, decimal value)
        {
            throw new NotImplementedException();
        }
        public decimal GetSalary(int userId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Salary> GetAllSalary()
        {
            throw new NotImplementedException();
        }
    }
}
