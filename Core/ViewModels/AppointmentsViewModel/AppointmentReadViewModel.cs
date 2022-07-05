using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;

namespace Core.ViewModels
{
    public class AppointmentReadViewModel : AppointmentBaseViewModel

    {
        public int Id { get; set; }

        public IEnumerable<ProcedureReadViewModel> Procedures { get; set; }
      = new List<ProcedureReadViewModel>();

        public IEnumerable<UserReadViewModel> Users { get; set; }
      = new List<UserReadViewModel>();

        public AnimalViewModel.AnimalViewModel AnimalViewModel { get; set; }

    }
}
