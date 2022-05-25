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

        public async Task DeleteSalaryAsync(int id)
        {
            _clinicContext.Remove(GetSalaryAsync(id));
            await _clinicContext.SaveChangesAsync();
        }

        public async Task<Salary?> GetSalaryAsync(int id)
        {
            return await _clinicContext.Salaries.FirstOrDefaultAsync(salary => salary.EmployeeId == id);
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            return await _clinicContext.Salaries.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            _clinicContext.Salaries.Update(salary);
            await _clinicContext.SaveChangesAsync();
        }
    }
}
