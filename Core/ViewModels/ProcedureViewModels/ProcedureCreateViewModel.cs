namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureCreateViewModel : ProcedureViewModelBase
{
    public IEnumerable<int> SpecializationIds { get; set; } = new List<int>();
}