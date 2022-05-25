using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Appointment/")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IEnumerable<Appointment>> GetAsync()
        {
            var appointments = await _appointmentService.GetAsync();
            return appointments;
        }

        [HttpGet("{AppointmentId}")]
        public async Task<Appointment> GetAsync([FromRoute] int appointmentId)
        {
            var appointment = await _appointmentService.GetAsync(appointmentId);
            return appointment;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Appointment appointment) // in future will use appointment Dto
        {
            await _appointmentService.CreateAsync(appointment);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(Appointment appointment) // in future will use appointment Dto
        {
            await _appointmentService.UpdateAsync(appointment);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromRoute] int appointmentId)
        {
            await _appointmentService.DeleteAsync(appointmentId);
            return Ok();
        }
    }
}
