using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;

namespace Core.ViewModels.SpecializationViewModels
{
    public class SpecializationViewModel : SpecializationBaseViewModel
    {
        public IEnumerable<ProcedureReadViewModel>? Procedures { get; set; }
        public IEnumerable<UserReadViewModel>? Users { get; set; }
    }
}
