using Core.Entities;
using Core.ViewModels.AppointmentsViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentCreateMapper : IViewModelMapper<AppointmentCreateViewModel, Appointment>
    {
        public Appointment Map(AppointmentCreateViewModel source)
        {
            var appointment = new Appointment()
            {
                Date = source.Date,
                Disease = source.Disease,
                MeetHasOccureding = source.MeetHasOccureding,
                AnimalId = source.AnimalId
                
            };
            return appointment;
        }
    }
}
