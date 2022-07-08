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
        private readonly IProcedureService _procedureService;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IProcedureService procedureService,
            IUserService userService,
            ILoggerManager logger)
        {
            _appointmentRepository = appointmentRepository;
            _procedureService = procedureService;
            _userService = userService;
            _logger = logger;  
        }

        public async Task CreateAsync(Appointment appointment, IEnumerable<int> procedureIds, IEnumerable<int> userIds, int animalId)
        {
            foreach (var procedureId in procedureIds)
            {
                appointment.AppointmentProcedures.Add(new AppointmentProcedure()
                {
                    Appointment = appointment,
                    Procedure = await _procedureService.GetByIdAsync(procedureId),
                });
            }

            foreach (var userId in userIds)
            {
                appointment.AppointmentUsers.Add(new AppointmentUser()
                {
                    Appointment = appointment,
                    User = await _userService.GetUserByIdAsync(userId),
                });
            }

            appointment.AnimalId = animalId;

            await _appointmentRepository.InsertAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo("Appointment was created in method CreateAsync");
        }

        public async Task DeleteAsync(int appointmentId)
        {
            Appointment? appointment = await GetAsync(appointmentId);

             _appointmentRepository.Delete(appointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo($"Appointment was deleted with Id {appointmentId} in method DeleteAsync");
        }

        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            var appointments = await _appointmentRepository.GetAsync(includeProperties: "AppointmentProcedures.Procedure,AppointmentUsers.User,Animal");
            _logger.LogInfo("Appointments were getted in method GetAsync");
            return appointments;
        }

        public async Task<Appointment> GetAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetById(appointmentId, "AppointmentProcedures.Procedure,AppointmentUsers.User,Animal");
           
            if (appointment is null)
            {
                _logger.LogWarn($"Appointment with id {appointmentId} does not exist");
                throw new NotFoundException($"Appointment with Id {appointmentId} does not exist");
            }

            _logger.LogInfo("Appointment was getted by appointmentId in method GetAsync");
            return appointment;
        }

        public async Task UpdateAppointmentProceduresAsync(int appointmentId, IEnumerable<int> procedureIds)
        {
            if (!procedureIds.Any())
                throw new ArgumentException("procedureId can't be empty");

            try
            {
                await _appointmentRepository.UpdateAppointmentProceduresAsync(appointmentId, procedureIds);
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarn("At least one of the procedures from the given list does not exist");
                throw new NotFoundException("At least one of the procedures from the given list does not exist");
            }
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo($"Updated procedure list of the appointments with Id {appointmentId}");
        }

        public async Task UpdateAppointmentUsersAsync(int appointmentId, IEnumerable<int> userIds)
        {
            if (!userIds.Any())
                throw new ArgumentException("userIds can't be empty");
            try
            {
                await _appointmentRepository.UpdateAppointmentUsersAsync(appointmentId, userIds);
            }
            catch (InvalidOperationException)
            {
                _logger.LogWarn("At least one of the users from the given list does not exist");
                throw new NotFoundException("At least one of the users from the given list does not exist");
            }
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo($"Updated user list of the appointments with Id {appointmentId}");
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await GetAsync(appointment.Id);

            existingAppointment.Date = appointment.Date;
            existingAppointment.MeetHasOccureding = appointment.MeetHasOccureding;
            existingAppointment.Disease = appointment.Disease;
            existingAppointment.AnimalId = appointment.AnimalId;
            //existingAppointment.AppointmentUsers = appointment.AppointmentUsers;
            //existingAppointment.AppointmentProcedures = appointment.AppointmentProcedures;

            _logger.LogInfo("Appointment was getted by appointmentId in method UpdateAsync");
        }
    }
}
