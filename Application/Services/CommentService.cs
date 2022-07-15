using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator.Parameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly ILoggerManager _loggerManager;
    private readonly UserManager<User> _userManager;

    public CommentService(
        ICommentRepository commentRepository, 
        ILoggerManager loggerManager, 
        UserManager<User> userManager)
    {
        _commentRepository = commentRepository;
        _loggerManager = loggerManager;
        _userManager = userManager;
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

    public async Task UpdateCommentAsync(Comment comment, User requestUser)
    {
        var updatingComment = await GetByIdAsync(comment.Id);
        if (updatingComment.AuthorId != requestUser.Id)
        {
            var message =
                $"Editing comment with different author: user id: {requestUser.Id}, author id: {comment.AuthorId}, comment id: {comment.Id}";
            _loggerManager.LogWarn(message);
            throw new BadRequestException(message);
        }
        updatingComment.Content = comment.Content;
        updatingComment.Edited = true;
        await _commentRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated comment with id {comment.Id}");
    }

    public async Task DeleteCommentAsync(int commentId, User requestUser)
    {
        var commentToRemove = await GetByIdAsync(commentId);
        var userRole = (await _userManager.GetRolesAsync(requestUser)).First();
        if (commentToRemove.AuthorId != requestUser.Id && userRole != "Admin")
        {
            var message =
                $"Deleting comment with different author: user id: {requestUser.Id}, author id: {commentToRemove.AuthorId}, comment id: {commentToRemove.Id}";
            _loggerManager.LogWarn(message);
            throw new BadRequestException(message);
        }

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

    public async Task<IEnumerable<Comment>> GetAllCommentsAsync(CommentsParameters parameters)
    {
        if (parameters.ArticleId != 0)
        {
            var publishedComments = await _commentRepository.GetAsync(
                includeProperties:"Author",
                filter: c => c.ArticleId == parameters.ArticleId);
            _loggerManager.LogInfo($"Found all comments for article with id {parameters.ArticleId}");
            return publishedComments;        
        }
        
        var comments = await _commentRepository.GetAsync(includeProperties:"Author");
        _loggerManager.LogInfo("Found all comments");
        return comments;
    }
}