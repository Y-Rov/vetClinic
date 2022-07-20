using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
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


            IQueryable<Salary> query = filter == null ? context.Salaries
                    .GroupBy(s => s.EmployeeId)
                    .Select(s => new Salary { EmployeeId = s.Key, Date = s.Max(x => x.Date)}) :
                 context.Salaries
                    .Where(filter)
                    .GroupBy(s => s.EmployeeId)
                    .Select(s => new Salary { EmployeeId = s.Key, Date = s.Max(x => x.Date) });

            var set = from s in context.Salaries
                      join q in query on s.Date equals q.Date
                      orderby s.EmployeeId
                      select new Salary { EmployeeId = s.EmployeeId, Value = s.Value };
  

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

        public async Task<PagedList<Salary>> GetAsync(
            SalaryParametrs parametrs,
            Expression<Func<Salary, bool>>? filter = null,
            Func<IQueryable<Salary>, IOrderedQueryable<Salary>>? orderBy = null,
            bool asNoTracking = true,
            string includeProperties = "")
        {
            var query = await GetQuery(filter, orderBy, includeProperties)
                .ToPagedListAsync(parametrs.PageNumber,parametrs.PageSize);

            return query;

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


    }
}
