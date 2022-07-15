using Core.Entities;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;
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
                AnimalId = source.AnimalId
            };

            return appointment;
        }
    }
}
