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
            await _clinicContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _clinicContext.Remove(appointment);
            await _clinicContext.SaveChangesAsync();    
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            return await _clinicContext.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            return await _clinicContext.Appointments.FirstOrDefaultAsync(app => app.Id == appointmentId);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
             _clinicContext.Appointments.Update(appointment);
            await _clinicContext.SaveChangesAsync();
        }
    }
}
