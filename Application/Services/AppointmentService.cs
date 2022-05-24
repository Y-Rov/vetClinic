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

        public AppointmentService(IAppointmentRepository appointmentRepository, ILoggerManager logger)
        {
            _appointmentRepository = appointmentRepository;
            _logger = logger;  
        }

        public async Task CreateAsync(Appointment appointment)
        {
            await _appointmentRepository.CreateAsync(appointment);
            _logger.LogInfo("Appointment was created in method CreateAsync");
        }

        public async Task DeleteAsync(int appointmentId)
        {
            Appointment? appointment = await _appointmentRepository.GetAsync(appointmentId);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with Id {appointmentId} does not exist");
            }

            _logger.LogInfo("Appointment was getted by appointmentId in method DeleteAsync");
            await _appointmentRepository.DeleteAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            var appointments = await _appointmentRepository.GetAsync();
            _logger.LogInfo("Appointments were getted in method GetAsync");
            return appointments;
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAsync(appointmentId);
            if (appointment != null)
            {
                _logger.LogInfo("Appointment was getted by appointmentId in method GetAsync");
                return appointment;
            }
            throw new NotFoundException($"Appointment with Id {appointmentId} does not exist");
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await _appointmentRepository.GetAsync(appointment.Id);
            if (existingAppointment != null)
            {
                _logger.LogInfo("Appointment was getted by appointmentId in method UpdateAsync");

                existingAppointment.Date = appointment.Date;
                existingAppointment.MeetHasOccureding= appointment.MeetHasOccureding;
                existingAppointment.Disease = appointment.Disease;
                existingAppointment.AnimalId = appointment.AnimalId;
                existingAppointment.AppointmentUsers = appointment.AppointmentUsers;
                existingAppointment.AppointmentProcedures = appointment.AppointmentProcedures; 

                _appointmentRepository.UpdateAsync(existingAppointment);
            }
            throw new NotFoundException($"Appointment with Id {appointment.Id} does not exist");
        }
    }
}
