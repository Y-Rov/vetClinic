using Core.ViewModel;

namespace Core.ViewModels;

public class ProcedureWithSpecializationsViewModel : ProcedureViewModelBase
{
    public List<SpecializationViewModel> Specializations { get; set; }
}