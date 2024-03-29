﻿using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.AppointmentsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/appointments/")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IViewModelMapper<AppointmentCreateViewModel, Appointment> _appointmentCreateMapper;
        private readonly IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>> _appointmentsViewModelMapper;
        private readonly IViewModelMapper<Appointment, AppointmentReadViewModel> _appointmentReadMapper;
        private readonly IViewModelMapper<AppointmentUpdateViewModel, Appointment> _appointmentUpdateMapper;
        private readonly IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AppointmentReadViewModel>> _appointmentPagedViewModel;

        public AppointmentController(
            IAppointmentService appointmentService,
            IViewModelMapper<AppointmentCreateViewModel, Appointment> appointmentCreateMapper,
            IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>> appointmentsViewModelMapper,
            IViewModelMapper<Appointment, AppointmentReadViewModel> appointmentReadMapper,
            IViewModelMapper<AppointmentUpdateViewModel, Appointment> appointmentUpdateMapper,
            IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AppointmentReadViewModel>> appointmentPagedViewModel
            )
        {
            _appointmentService = appointmentService;
            _appointmentsViewModelMapper = appointmentsViewModelMapper;
            _appointmentCreateMapper = appointmentCreateMapper;
            _appointmentReadMapper = appointmentReadMapper;
            _appointmentUpdateMapper = appointmentUpdateMapper; 
            _appointmentPagedViewModel = appointmentPagedViewModel; 
        }

        [Authorize]
        [HttpGet]
        public async Task<PagedReadViewModel<AppointmentReadViewModel>> GetAsync([FromQuery] AppointmentParameters parameter)
        {
            var appointments = await _appointmentService.GetAsync(parameter);

            var appointmentsViewModel = _appointmentPagedViewModel.Map(appointments);

            return appointmentsViewModel;
        }

        [Authorize]
        [HttpGet("{appointmentId:int:min(1)}")]
        public async Task<AppointmentReadViewModel> GetAsync([FromRoute] int appointmentId)
        {
           var appointment = await _appointmentService.GetAsync(appointmentId);

           var appointmentViewModel = _appointmentReadMapper.Map(appointment);

            return appointmentViewModel;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAsync(AppointmentCreateViewModel appointmentViewModel)
        {
            var appointment = _appointmentCreateMapper.Map(appointmentViewModel);

            await _appointmentService.CreateAsync(appointment, appointmentViewModel.ProcedureIds,appointmentViewModel.UserIds, appointmentViewModel.AnimalId );

            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutAsync(AppointmentUpdateViewModel appointmentViewModel) 
        {
            var appointment = _appointmentUpdateMapper.Map(appointmentViewModel);

            await _appointmentService.UpdateAsync(appointment);
            await _appointmentService.UpdateAppointmentProceduresAsync(appointmentViewModel.Id, appointmentViewModel.ProcedureIds);
            await _appointmentService.UpdateAppointmentUsersAsync(appointmentViewModel.Id, appointmentViewModel.UserIds);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{appointmentId:int:min(1)}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int appointmentId)
        {
            await _appointmentService.DeleteAsync(appointmentId);

            return NoContent();
        }
    }
}
