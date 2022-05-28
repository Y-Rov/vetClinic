using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
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
                    MeetHasOccured = appointmentViewModel.MeetHasOccureding,
                    AnimalId = appointmentViewModel.AnimalId
                };

                return appointment;
        }
    }
}
