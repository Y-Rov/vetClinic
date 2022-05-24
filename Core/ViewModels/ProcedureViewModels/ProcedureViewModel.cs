using Core.ViewModel;

namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureViewModel : ProcedureViewModelBase
{
    public int Id { get; set; }
    public IEnumerable<SpecializationViewModel> Specializations { get; set; } 
}