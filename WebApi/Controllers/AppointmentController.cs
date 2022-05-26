using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using WebApi.Validators;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/appointments/")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IViewModelMapper<Appointment, AppointmentViewModel> _appointmentViewModelMapper;
        private readonly IEnumerableViewModelMapper<Appointment, AppointmentViewModel> _appointmentsViewModelMapper;
        private readonly IViewModelMapper<AppointmentViewModel, Appointment> _appointmentMapper;
        private readonly AppointmentViewModelValidator _appointmentValidator;

        public AppointmentController(
            IAppointmentService appointmentService,
            IViewModelMapper<Appointment, AppointmentViewModel> appointmentViewModelMapper,
            IEnumerableViewModelMapper<Appointment, AppointmentViewModel> appointmentsViewModelMapper,
            IViewModelMapper<AppointmentViewModel, Appointment> appointmentMapper,
            AppointmentViewModelValidator appointmentValidator
            )
        {
            _appointmentService = appointmentService;
            _appointmentViewModelMapper = appointmentViewModelMapper;
            _appointmentsViewModelMapper = appointmentsViewModelMapper;
            _appointmentMapper = appointmentMapper;
            _appointmentValidator = appointmentValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<AppointmentViewModel>> GetAsync()
        {
            var appointments = await _appointmentService.GetAsync();

            var appointmentsViewModel = _appointmentsViewModelMapper.Map(appointments);

            return appointmentsViewModel;
        }

        [HttpGet("{AppointmentId:int:min(1)}")]
        public async Task<AppointmentViewModel> GetAsync([FromRoute] int appointmentId)
        {
           var appointment = await _appointmentService.GetAsync(appointmentId);

           var appointmentViewModel = _appointmentViewModelMapper.Map(appointment);

            return appointmentViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(AppointmentViewModel appointmentViewModel)
        {
            var validResult = await _appointmentValidator.ValidateAsync(appointmentViewModel);
            
            if (!validResult.IsValid)
            {
                throw new BadRequestException(validResult.Errors.ToString());
            }

            var appointment = _appointmentMapper.Map(appointmentViewModel);

            await _appointmentService.CreateAsync(appointment);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(AppointmentViewModel appointmentViewModel) 
        {
            var validResult = await _appointmentValidator.ValidateAsync(appointmentViewModel);
            
            if (!validResult.IsValid)
            {
                throw new BadRequestException(validResult.Errors.ToString());
            }

            var appointment = _appointmentMapper.Map(appointmentViewModel);

            await _appointmentService.UpdateAsync(appointment);

            return Ok();
        }

        [HttpDelete("{AppointmentId:int:min(1)}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int appointmentId)
        {
            await _appointmentService.DeleteAsync(appointmentId);

            return Ok();
        }
    }
}
