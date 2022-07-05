namespace Core.ViewModels.ArticleViewModels;

public class ReadArticleViewModel : ArticleViewModelBase
{
    public int Id { get; set; }
    public bool Edited { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AuthorName { get; set; }
    public int AuthorId { get; set; }
}