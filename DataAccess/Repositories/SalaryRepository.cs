using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SalaryRepository : Repository<Salary>, ISalaryRepository
    {
        readonly ClinicContext context;
        public SalaryRepository(ClinicContext context): base(context) 
        {
            this.context = context;
        }

        public new async Task<Salary?> GetById(int id, string includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
                return await context.Set<Salary>()
                    .Where(s => s.EmployeeId == id)
                    .OrderByDescending(s => s.Date)
                    .FirstOrDefaultAsync();

            var result = await context.Set<Salary>()
                                    .Where(s => s.EmployeeId == id)
                                    .OrderByDescending(s => s.Date)
                                    .FirstOrDefaultAsync();

            IQueryable<Salary> set = context.Set<Salary>();

            set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            return await set.FirstOrDefaultAsync(entity => entity == result);
        }

        public async Task<IEnumerable<int>> GetEmployees()
        {

            var allEmployeesId = await context.UserRoles
                .Where(x => x.RoleId!=4)
                .Select(x => x.UserId)
                .ToListAsync();
           

            return allEmployeesId;
        }

    }
}
