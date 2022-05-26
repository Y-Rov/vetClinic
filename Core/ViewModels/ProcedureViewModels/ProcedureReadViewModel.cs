using Core.ViewModels.SpecializationViewModels;

namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureReadViewModel : ProcedureViewModelBase
{
    public IEnumerable<SpecializationViewModel> Specializations { get; set; }
        = new List<SpecializationViewModel>();
}