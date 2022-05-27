using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILoggerManager _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository, 
            ILoggerManager logger)
        {
            _appointmentRepository = appointmentRepository;
            _logger = logger;  
        }

        public async Task CreateAsync(Appointment appointment)
        {
            await _appointmentRepository.CreateAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo("Appointment was created in method CreateAsync");
        }

        public async Task DeleteAsync(int appointmentId)
        {
            Appointment? appointment = await GetAsync(appointmentId);

            _logger.LogInfo("Appointment was getted by appointmentId in method DeleteAsync");
            await _appointmentRepository.DeleteAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            var appointments = await _appointmentRepository.GetAsync();
            if (appointments is null)
            {
                _logger.LogWarn("Appointments is null");
                throw new NotFoundException("Appointments is null");
            }
            _logger.LogInfo("Appointments were getted in method GetAsync");
            return appointments;
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAsync(appointmentId);
           
            if (appointment is null)
            {
                _logger.LogWarn($"Appointment with id {appointmentId} does not exist");
                throw new NotFoundException($"Appointment with Id {appointmentId} does not exist");
            }

            _logger.LogInfo("Appointment was getted by appointmentId in method GetAsync");
            return appointment;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await GetAsync(appointment.Id);

            existingAppointment.Date = appointment.Date;
            existingAppointment.MeetHasOccureding = appointment.MeetHasOccureding;
            existingAppointment.Disease = appointment.Disease;
            existingAppointment.AnimalId = appointment.AnimalId;
            existingAppointment.AppointmentUsers = appointment.AppointmentUsers;
            existingAppointment.AppointmentProcedures = appointment.AppointmentProcedures;

            _appointmentRepository.UpdateAsync(existingAppointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo("Appointment was getted by appointmentId in method UpdateAsync");
        }
    }
}
