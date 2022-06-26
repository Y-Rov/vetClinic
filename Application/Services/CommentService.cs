using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILoggerManager _loggerManager;

    public CommentService(
        ICommentRepository commentRepository, 
        ILoggerManager loggerManager)
    {
        _commentRepository = commentRepository;
        _loggerManager = loggerManager;
    }
    
    public async Task CreateCommentAsync(Comment comment)
    {
        try
        {
            await _commentRepository.InsertAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _loggerManager.LogWarn($"user with id {comment.AuthorId} not found");
            throw new NotFoundException($"user with id {comment.AuthorId} not found");
        }
        
        _loggerManager.LogInfo($"Created new comment with content {comment.Content}");
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _commentRepository.Update(comment);
        await _commentRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated comment with id {comment.Id}");
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var commentToRemove = await GetByIdAsync(commentId);

        _commentRepository.Delete(commentToRemove);
        await _commentRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted comment with id {commentId}");
    }

    public async Task<Comment> GetByIdAsync(int commentId)
    {
        var comment = await _commentRepository.GetById(commentId, includeProperties:"Author");
        if (comment is null)
        {
            _loggerManager.LogWarn($"Comment with id {commentId} does not exist");
            throw new NotFoundException($"Comment with id {commentId} does not exist");
        }
        
        _loggerManager.LogInfo($"Found comment with id {commentId}");
        return comment;
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.GetAsync(includeProperties:"Author");
        _loggerManager.LogInfo("Found all comments");
        return comments;
    }

    public async Task<IEnumerable<Comment>> GetAllArticleCommentsAsync(int articleId)
    {
        var publishedComments = await _commentRepository.GetAsync(
            includeProperties:"Author",
            filter: c => c.ArticleId == articleId);
        _loggerManager.LogInfo($"Found all comments for article with id {articleId}");
        return publishedComments;
    }
}