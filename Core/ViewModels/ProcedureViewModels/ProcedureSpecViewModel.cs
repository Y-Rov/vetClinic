using Core.ViewModels.SpecializationViewModels;

namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureSpecViewModel : ProcedureViewModelBase
{
    public int Id { get; set; }

    public IEnumerable<SpecializationViewModel> Specializations { get; set; }
        = new List<SpecializationViewModel>();
}