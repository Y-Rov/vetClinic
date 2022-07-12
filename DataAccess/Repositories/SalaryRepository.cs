using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class SalaryRepository : Repository<Salary>, ISalaryRepository
    {
        readonly ClinicContext context;
        public SalaryRepository(ClinicContext context): base(context) 
        {
            this.context = context;
        }

        public new IQueryable<Salary> GetQuery(
            Expression<Func<Salary, bool>>? filter,
            Func<IQueryable<Salary>, IOrderedQueryable<Salary>>? orderBy,
            string includeProperties = "")
        {
            IQueryable<Salary> set = filter == null ? context.Salaries
                    .GroupBy(s => s.EmployeeId)
                    .Select(s => new Salary { EmployeeId = s.Key, Value = s.Max(x => x.Value) }) :
                 context.Salaries
                    .Where(filter)
                    .GroupBy(s => s.EmployeeId)
                    .Select(s => new Salary { EmployeeId = s.Key, Value = s.Max(x => x.Value) });

            if (!string.IsNullOrEmpty(includeProperties))
            {
                set = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(set, (current, includeProperty)
                        => current.Include(includeProperty));
            }

            if (orderBy != null)
            {
                set = orderBy(set);
            }

            return set;
        }

        public new async Task<IList<Salary>> GetAsync(
            Expression<Func<Salary, bool>>? filter = null,
            Func<IQueryable<Salary>, IOrderedQueryable<Salary>>? orderBy = null,
            string includeProperties = "",
            bool asNoTracking = false)
        {
            var query = GetQuery(filter, orderBy, includeProperties);

            if (asNoTracking)

            {
                var noTrackingResult = await query.AsNoTracking()
                    .ToListAsync();

                return noTrackingResult;
            }

            var trackingResult = await query.ToListAsync();
            return trackingResult;

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

        //public async Task<IEnumerable<int>> GetEmployees()
        //{

        //    var allEmployeesId = await context.UserRoles
        //        .Where(x => x.RoleId!=4)
        //        .Select(x => x.UserId)
        //        .ToListAsync();
           

        //    return allEmployeesId;
        //}

    }
}
