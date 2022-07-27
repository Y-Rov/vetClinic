using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IProcedureService _procedureService;
        private readonly IAppointmentUserRepository _appointmentUserRepository;
        private readonly IAppointmentProcedureRepository _appointmentProcedureRepository;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IProcedureService procedureService,
            IUserService userService,
            IAppointmentUserRepository appointmentUserRepository,
            IAppointmentProcedureRepository appointmentProcedureRepository,
            ILoggerManager logger)
        {
            _appointmentRepository = appointmentRepository;
            _procedureService = procedureService;
            _userService = userService;
            _logger = logger;
            _appointmentProcedureRepository = appointmentProcedureRepository;
            _appointmentUserRepository = appointmentUserRepository;
        }

        public async Task CreateAsync(Appointment appointment, IEnumerable<int> procedureIds, IEnumerable<int> userIds, int animalId)
        {
            foreach (var procedureId in procedureIds)
            {
                appointment.AppointmentProcedures.Add(new AppointmentProcedure()
                {
                    Appointment = appointment,
                    Procedure = await _procedureService.GetByIdAsync(procedureId)
                });
            }

            foreach (var userId in userIds)
            {
                appointment.AppointmentUsers.Add(new AppointmentUser()
                {
                    Appointment = appointment,
                    User = await _userService.GetUserByIdAsync(userId)
                });
            }

            appointment.AnimalId = animalId;

            await _appointmentRepository.InsertAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo("Appointment was created in method CreateAsync");
        }

        public async Task DeleteAsync(int appointmentId)
        {
            var appointment = await GetAsync(appointmentId);

             _appointmentRepository.Delete(appointment);
            await _appointmentRepository.SaveChangesAsync();
            _logger.LogInfo($"Appointment was deleted with Id {appointmentId} in method DeleteAsync");
        }

        public async Task<PagedList<Appointment>> GetAsync(AppointmentParameters parameters)
        {
            var appointments = await _appointmentRepository.GetAsync(parameters, includeProperties: "AppointmentProcedures.Procedure,AppointmentUsers.User,Animal");

            _logger.LogInfo("Getting all Appointments in method GetAsync");
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

            _logger.LogInfo($"Appointment with {appointmentId} was fetched in method GetAsync");
            return appointment;
        }

        public async Task UpdateAppointmentProceduresAsync(int appointmentId, IEnumerable<int> procedureIds)
        {
            if (procedureIds.Any())
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

                await _appointmentRepository.SaveChangesAsync();
                _logger.LogInfo($"Updated procedure list of the appointments with Id {appointmentId}");
            }
        }

        public async Task UpdateAppointmentUsersAsync(int appointmentId, IEnumerable<int> userIds)
        {
            if (userIds.Any())
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

                await _appointmentRepository.SaveChangesAsync();

                _logger.LogInfo($"Updated user list of the appointments with Id {appointmentId}");

            }
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await GetAsync(appointment.Id);

            existingAppointment.Date = appointment.Date;
            existingAppointment.MeetHasOccureding = appointment.MeetHasOccureding;
            existingAppointment.Disease = appointment.Disease;
            existingAppointment.AnimalId = appointment.AnimalId;
          
            _logger.LogInfo($"Updated Appointment by {appointment.Id} in method UpdateAsync");
        }
    }
}
