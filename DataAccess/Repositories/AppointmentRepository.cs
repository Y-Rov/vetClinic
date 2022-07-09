using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AppointmentRepository : Repository<Appointment> , IAppointmentRepository
    {

        public AppointmentRepository(ClinicContext context) : base(context)
        {
        }
    }
}
