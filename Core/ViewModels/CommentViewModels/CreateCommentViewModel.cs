namespace Core.ViewModels.CommentViewModels;

public class CreateCommentViewModel : CommentViewModelBase
{
    public int AuthorId { get; set; }
    public int ArticleId { get; set; }
}