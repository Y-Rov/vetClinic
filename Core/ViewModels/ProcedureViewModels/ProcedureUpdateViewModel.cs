namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureUpdateViewModel : ProcedureViewModelBase
{
    public int Id { get; set; }
    public IEnumerable<int> SpecializationIds { get; set; } = new List<int>();
}