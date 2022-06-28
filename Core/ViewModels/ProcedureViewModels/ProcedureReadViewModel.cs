using Core.ViewModels.SpecializationViewModels;

namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureReadViewModel : ProcedureViewModelBase
{
    public int Id { get; set; }
    public IEnumerable<SpecializationBaseViewModel> Specializations { get; set; }
        = new List<SpecializationBaseViewModel>();
}