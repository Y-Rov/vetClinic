using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AppointmentRepository : Repository<Appointment> , IAppointmentRepository
    {
        private readonly IAppointmentUserRepository _appointmentUserRepository;
        private readonly IAppointmentProcedureRepository _appointmentProcedureRepository;

        public AppointmentRepository(ClinicContext context, 
           IAppointmentUserRepository appointmentUserRepository, 
           IAppointmentProcedureRepository appointmentProcedureRepositor) : base(context)
        {
            _appointmentProcedureRepository = appointmentProcedureRepositor;
            _appointmentUserRepository = appointmentUserRepository;
        }

        public async Task UpdateAppointmentProceduresAsync(int appointmentId, IEnumerable<int> procedureIds)
        {
            var existing = await _appointmentProcedureRepository.GetAsync(
                filter: app => app.AppointmentId == appointmentId);

            foreach (var app in existing) 
            {
                _appointmentProcedureRepository.Delete(app);
            }

            await _appointmentProcedureRepository.SaveChangesAsync();

            foreach (var procedureId in procedureIds) 
            {
                await _appointmentProcedureRepository.InsertAsync(new AppointmentProcedure()
                {
                    AppointmentId = appointmentId,
                    ProcedureId = procedureId
                });
            }
        }

        public async Task UpdateAppointmentUsersAsync(int appointmentId, IEnumerable<int> userIds)
        {
            var existing = await _appointmentUserRepository.GetAsync(
                filter: app => app.AppointmentId == appointmentId);
            foreach (var app in existing)
            {
                _appointmentUserRepository.Delete(app);
            }
            await _appointmentUserRepository.SaveChangesAsync();
            foreach (var userId in userIds)
            {
                await _appointmentUserRepository.InsertAsync(new AppointmentUser()
                {
                    AppointmentId = appointmentId,
                    UserId = userId
                });
            }
        }
    }
}
