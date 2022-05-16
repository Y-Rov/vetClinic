using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IFinancialService
    {
        public void GenerateFinStatementForMonth();
        public void GenerateFinStatementForHalfOfYear();
        public void GenerateFinStatementForYear();
        public void SetSalary(int userId, decimal value);
        public void ChangeSalary(int userId, decimal value);
        public decimal GetSalary(int userId);
        public IEnumerable<Salary> GetAllSalary();

    }
}
