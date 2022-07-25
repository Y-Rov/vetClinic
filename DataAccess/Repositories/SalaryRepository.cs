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

        public IQueryable<Salary> GetQuery(
            Expression<Func<Salary, bool>>? filter)
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
                      where s.Value != 0
                      select new Salary { EmployeeId = s.EmployeeId, Value = s.Value, Date = s.Date };

            return set;
        }

        public async Task<PagedList<Salary>> GetAsync(
            SalaryParametrs parametrs,
            Expression<Func<Salary, bool>>? filter = null)
        {
            var query = await GetQuery(filter)
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


        public async Task<Salary?> GetByIdForStatement(int id, Expression<Func<Salary, bool>> filter)
        {
            var query = context.Salaries
                .Where(s => s.EmployeeId == id)
                .OrderBy(s => s.Date)
                .Select(s=>new Salary{EmployeeId=s.EmployeeId,Id=s.Id,Value=s.Value, Date = s.Date });

            var res = await query
                .Where(filter)
                .FirstOrDefaultAsync();
            return res;
        }

        public async Task<IEnumerable<Salary>> GetAllForStatement(Expression<Func<Salary, bool>> filter)
        {
            var query = context.Salaries
                    .Where(filter)
                    .GroupBy(s => s.EmployeeId)
                    .Select(s => new Salary { EmployeeId = s.Key, Date = s.Max(x => x.Date) });

            var set = from s in context.Salaries
                      join q in query on s.Date equals q.Date
                      orderby s.EmployeeId
                      select new Salary { EmployeeId = s.EmployeeId, Value = s.Value, Date = s.Date };

            return await set.ToListAsync();
        }

    }
}
