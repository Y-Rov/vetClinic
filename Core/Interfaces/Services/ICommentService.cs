using Core.Entities;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services;

public interface ICommentService
{
    Task CreateCommentAsync(Comment comment);
    Task UpdateCommentAsync(Comment comment, User requestUser);
    Task DeleteCommentAsync(int commentId, User requestUser);
    Task<Comment> GetByIdAsync(int commentId);
    Task<IEnumerable<Comment>> GetAllCommentsAsync(CommentsParameters parameters);
}