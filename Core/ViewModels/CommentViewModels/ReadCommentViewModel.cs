namespace Core.ViewModels.CommentViewModels;

public class ReadCommentViewModel : CommentViewModelBase
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Edited { get; set; }
    public int ArticleId { get; set; }
}