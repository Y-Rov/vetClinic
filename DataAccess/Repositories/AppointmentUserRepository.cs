using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AppointmentUserRepository : Repository<AppointmentUser>, IAppointmentUserRepository
    {
        public AppointmentUserRepository(ClinicContext context) : base(context) { }
    }
}
