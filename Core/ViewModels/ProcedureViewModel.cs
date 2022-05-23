namespace Core.ViewModel.ProcedureViewModels;

public class ProcedureViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int DurationInMinutes { get; set; }
}