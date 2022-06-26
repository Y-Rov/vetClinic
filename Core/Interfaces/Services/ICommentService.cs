using Core.Entities;

namespace Core.Interfaces.Services;

public interface ICommentService
{
    Task CreateCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(int commentId);
    Task<Comment> GetByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetAllCommentsAsync();
    Task<IEnumerable<Comment>> GetAllArticleCommentsAsync(int articleId);
}