using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly ClinicContext _clinicContext;

        public SalaryRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            await _clinicContext.Salaries.AddAsync(salary);
        }

        public async Task<Salary?> GetSalaryByUserIdAsync(int id)
        {
            var result = await _clinicContext.Salaries
                .SingleOrDefaultAsync(salary => salary.EmployeeId == id);
            return result;
        }
        public async Task DeleteSalaryAsync(Salary salary)
        {
            _clinicContext.Salaries.Remove(salary);
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            var result = await _clinicContext.Salaries.ToListAsync();
            return result;
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            _clinicContext.Salaries.Update(salary);
        }
        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }

    }
}
