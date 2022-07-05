using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AppointmentProcedureRepository : Repository<AppointmentProcedure>, IAppointmentProcedureRepository
    {
        public AppointmentProcedureRepository(ClinicContext context) : base(context) { }
    }
}