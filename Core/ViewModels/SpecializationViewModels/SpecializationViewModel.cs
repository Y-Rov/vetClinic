using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;

namespace Core.ViewModels.SpecializationViewModels
{
    public class SpecializationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProcedureReadViewModel>? Procedures { get; set; }
        public IEnumerable<UserReadViewModel>? Users { get; set; }
    }
}
