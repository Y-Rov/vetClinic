using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMapper
{
    public class AppointmentViewModelMapper : IViewModelMapper<AppointmentViewModel, Appointment>
    {
        public Appointment Map(AppointmentViewModel appointmentViewModel)
        {
                var appointment = new Appointment()
                {
                    Id = appointmentViewModel.Id,
                    Date = appointmentViewModel.Date,
                    Disease = appointmentViewModel.Disease,
                    MeetHasOccureding = appointmentViewModel.MeetHasOccureding
                };

                return appointment;
        }
    }
}
