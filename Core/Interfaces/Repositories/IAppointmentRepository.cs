using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<PagedList<Appointment>> GetAsync(
            AppointmentParameters parametrs,
            Expression<Func<Appointment, bool>>? filter = null,
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>? orderBy = null,
            bool asNoTracking = true,
            string includeProperties = "");
    }
}
