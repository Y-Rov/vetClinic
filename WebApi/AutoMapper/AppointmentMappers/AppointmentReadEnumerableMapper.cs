using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentReadEnumerableMapper : IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>
    {
        private readonly IViewModelMapper<Appointment, AppointmentReadViewModel> _readMapper;
        
        public AppointmentReadEnumerableMapper(IViewModelMapper<Appointment, AppointmentReadViewModel> readMapper)
        {
            _readMapper = readMapper;
        }

        public IEnumerable<AppointmentReadViewModel> Map(IEnumerable<Appointment> source)
        {
            var appointmentViewModels = source.Select(app => _readMapper.Map(app)).ToList();
            return appointmentViewModels;
        }

        private AppointmentReadViewModel GetAppointmentViewModel(Appointment appointment)
        {
            var appointmentViewModel = new AppointmentReadViewModel()
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