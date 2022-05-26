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
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            return await _clinicContext.Appointments
                .Include(appointment => appointment.AppointmentProcedures)
                .Include(appointment => appointment.AppointmentUsers).ToListAsync();
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            return await _clinicContext.Appointments
                .Include(appointment => appointment.AppointmentProcedures)
                .Include(appointment => appointment.AppointmentUsers)
                .FirstOrDefaultAsync(app => app.Id == appointmentId);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
             _clinicContext.Appointments.Update(appointment); 
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
