using Core.Entities;
using Core.ViewModels;
using Core.ViewModels.AppointmentsViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentUpdateModelMapper : IViewModelMapper<AppointmentUpdateViewModel, Appointment>
    { 
        public Appointment Map(AppointmentUpdateViewModel source)
        {
            var appointment = new Appointment()
            {
                Id = source.Id,
                Date = source.Date,
                Disease = source.Disease,
                MeetHasOccureding = source.MeetHasOccureding,
            };

            return appointment;
        }
    }
}
