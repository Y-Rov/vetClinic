using Core.ViewModel;

namespace Core.ViewModels;

public class ProcedureWithSpecializationsViewModel : ProcedureViewModelBase
{
    public int Id { get; set; }
    public List<SpecializationViewModel> Specializations { get; set; }
}