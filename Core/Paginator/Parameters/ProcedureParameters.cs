namespace Core.Paginator.Parameters;

public class ProcedureParameters : ElementParameters
{
    public string? FilterParam { get; init; }
    public string? OrderByParam { get; set; }
    public string? OrderByDirection { get; set; }
}