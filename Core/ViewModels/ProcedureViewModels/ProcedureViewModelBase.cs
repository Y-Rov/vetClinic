namespace Core.ViewModels.ProcedureViewModels;

public class ProcedureViewModelBase
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int DurationInMinutes { get; set; }
}