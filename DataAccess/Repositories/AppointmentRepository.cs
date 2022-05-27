using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ClinicContext _clinicContext;

        public AppointmentRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            await _clinicContext.AddAsync(appointment);
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _clinicContext.Remove(appointment);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            var appointment = await _clinicContext.Appointments
                .Include(appointment => appointment.AppointmentProcedures)
                .ThenInclude(appointment => appointment.Procedure)
                .Include(appointment => appointment.AppointmentUsers).ToListAsync();
            return appointment;
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            var appointment = await _clinicContext.Appointments
                .Include(appointment => appointment.AppointmentProcedures)
                .ThenInclude(appointment => appointment.Procedure)
                .Include(appointment => appointment.AppointmentUsers)
                .FirstOrDefaultAsync(app => app.Id == appointmentId);
            return appointment;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
             _clinicContext.Appointments.Update(appointment);
             await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
