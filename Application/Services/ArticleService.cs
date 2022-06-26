using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILoggerManager _loggerManager;

    public ArticleService(
        IArticleRepository articleRepository,
        ILoggerManager loggerManager)
    {
        _articleRepository = articleRepository;
        _loggerManager = loggerManager;
    }
    
    public async Task CreateArticleAsync(Article article)
    {
        try
        {
            await _articleRepository.InsertAsync(article);
            await _articleRepository.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _loggerManager.LogWarn($"user with id {article.AuthorId} not found");
            throw new NotFoundException($"user with id {article.AuthorId} not found");
        }
        
        _loggerManager.LogInfo($"Created new article with title {article.Title}");
    }

    public async Task UpdateArticleAsync(Article article)
    {
        _articleRepository.Update(article);
        await _articleRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated article with id {article.Id}");
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        var articleToRemove = await GetByIdAsync(articleId);

        _articleRepository.Delete(articleToRemove);
        await _articleRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted article with id {articleId}");
    }

    public async Task<Article> GetByIdAsync(int articleId)
    {
        var article = await _articleRepository.GetById(articleId, includeProperties:"Author");
        if (article is null)
        {
            _loggerManager.LogWarn($"Article with id {articleId} does not exist");
            throw new NotFoundException($"Article with id {articleId} does not exist");
        }
        
        _loggerManager.LogInfo($"Found article with id {articleId}");
        return article;
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync()
    {
        var articles = await _articleRepository.GetAsync(includeProperties:"Author");
        _loggerManager.LogInfo("Found all articles");
        return articles;
    }

    public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
    {
        var publishedArticles = await _articleRepository.GetAsync(
            includeProperties:"Author",
            filter: article => article.Published);
        _loggerManager.LogInfo("Found all published articles");
        return publishedArticles;
    }
}