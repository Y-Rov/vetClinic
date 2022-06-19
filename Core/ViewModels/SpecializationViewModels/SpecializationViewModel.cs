using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;

namespace Core.ViewModels.SpecializationViewModels
{
    public class SpecializationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProcedureViewModelBase>? Procedures { get; set; }
        public IEnumerable<UserBaseViewModel>? Users { get; set; }
    }
}
