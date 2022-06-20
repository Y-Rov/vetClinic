using Core.Entities;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.AppointmentMappers
{
    public class AppointmentReadMapper : IViewModelMapper<Appointment, AppointmentReadViewModel>
    {
        private readonly IViewModelMapper<Animal, AnimalViewModel> _animalMapper;

        IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> _procedureMapper;

        IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> _userMapper;

        public AppointmentReadMapper(
           IViewModelMapper<Animal, AnimalViewModel> animalMapper,
           IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> procedureMapper,
           IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>> userMapper
            )
        {
            _animalMapper=animalMapper;
            _procedureMapper=procedureMapper;
            _userMapper=userMapper;
        }

        public AppointmentReadViewModel Map(Appointment appointment)
        {
            var appointmentViewModel = new AppointmentReadViewModel()
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Disease = appointment.Disease,
                MeetHasOccureding = appointment.MeetHasOccureding,
               
            };

            appointmentViewModel.Procedures = _procedureMapper.Map(appointment.AppointmentProcedures.Select(ap => ap.Procedure));
            appointmentViewModel.Users = _userMapper.Map(appointment.AppointmentUsers.Select(au => au.User));
            appointmentViewModel.AnimalViewModel = _animalMapper.Map(appointment.Animal);

            return appointmentViewModel;
        }
    }
}
