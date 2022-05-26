using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentsMapper : IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentViewModel>>
    {
        public IEnumerable<AppointmentViewModel> Map(IEnumerable<Appointment> source)
        {
            var appointmentViewModels = source.Select(GetAppointmentViewModel).ToList();
            return appointmentViewModels;
        }

        private AppointmentViewModel GetAppointmentViewModel(Appointment appointment) 
        {
            var appointmentViewModel = new AppointmentViewModel()
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Disease = appointment.Disease,
                MeetHasOccureding = appointment.MeetHasOccureding
            };

            return appointmentViewModel;
        }
    }
}
