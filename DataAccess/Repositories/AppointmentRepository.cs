using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AppointmentRepository : Repository<Appointment> , IAppointmentRepository
    {

        public AppointmentRepository(ClinicContext context) : base(context)
        {
        }
    }
}
