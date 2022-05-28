using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentMapper : IViewModelMapper<Appointment, AppointmentViewModel>
    {
        public AppointmentViewModel Map(Appointment appointment)
        {
            var appointmentViewModel = new AppointmentViewModel()
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Disease = appointment.Disease,
                MeetHasOccureding = appointment.MeetHasOccured,
                AnimalId = appointment.AnimalId
            };

            return appointmentViewModel;
        }
    }
}
