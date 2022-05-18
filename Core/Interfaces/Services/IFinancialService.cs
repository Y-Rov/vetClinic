using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IFinancialService
    {
        public void GenerateFinancialStatementForMonth();
        public void GenerateFinancialStatementForHalfOfYear();
        public void GenerateFinancialStatementForYear();
        public decimal GetSalaryById(int id);
        public decimal SetSalaryById(int id, decimal value);
        public IEnumerable<Salary> GetSalaries();
    }
}
