namespace Core.ViewModel.ProcedureDTOs;

public class ProcedureDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public int DurationInMinutes { get; set; }
}