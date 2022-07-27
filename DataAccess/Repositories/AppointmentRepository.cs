using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ClinicContext context) : base(context)
        {
        }

        public new async Task<PagedList<Appointment>> GetAsync(
           AppointmentParameters parametrs,
           Expression<Func<Appointment, bool>>? filter = null,
           Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>? orderBy = null,
           bool asNoTracking = true,
           string includeProperties = "")
        {
            var query = await GetQuery(filter, orderBy, includeProperties)
                .ToPagedListAsync(parametrs.PageNumber, parametrs.PageSize);

            return query;

        }

    }
}
