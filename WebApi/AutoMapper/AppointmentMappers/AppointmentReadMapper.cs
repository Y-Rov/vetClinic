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

        //private readonly IViewModelMapper<IEnumerable<AppointmentProcedure>, IEnumerable<ProcedureReadViewModel>> _procedureMapper;

        private readonly IViewModelMapper<User, UserReadViewModel> _userMapper;

        private readonly IViewModelMapper<Procedure, ProcedureReadViewModel> _pMapper;

        public AppointmentReadMapper(
           IViewModelMapper<Animal, AnimalViewModel> animalMapper,
           //IViewModelMapper<IEnumerable<AppointmentProcedure>, IEnumerable<ProcedureReadViewModel>> procedureMapper,
           IViewModelMapper<User, UserReadViewModel> userMapper,
            IViewModelMapper<Procedure, ProcedureReadViewModel> pMapper
            )
        {
            _animalMapper=animalMapper;
            //_procedureMapper = procedureMapper;
            _userMapper = userMapper;
            _pMapper=pMapper;   
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


            appointmentViewModel.Procedures = appointment.AppointmentProcedures.Select(p => _pMapper.Map(p.Procedure));
            appointmentViewModel.Users = appointment.AppointmentUsers.Select(p => _userMapper.Map(p.User));

            //appointmentViewModel.Procedures = _procedureMapper.Map(appointment.AppointmentProcedures);
            //appointmentViewModel.Users = _userMapper.Map(appointment.AppointmentUsers);
            appointmentViewModel.AnimalViewModel = _animalMapper.Map(appointment.Animal);

            return appointmentViewModel;
        }
    }
}
