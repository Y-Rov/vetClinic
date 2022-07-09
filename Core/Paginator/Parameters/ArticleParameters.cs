namespace Core.Paginator.Parameters;

public class ArticleParameters 
{
    public string? FilterParam { get; init; }
    public string? OrderByParam { get; set; }
    public string? OrderByDirection { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }
}