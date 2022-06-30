using Core.Entities;

namespace Core.Interfaces.Services;

public interface IArticleService
{
    Task CreateArticleAsync(Article article);
    Task UpdateArticleAsync(Article article);
    Task DeleteArticleAsync(int articleId);
    Task<Article> GetByIdAsync(int articleId);
    Task<IEnumerable<Article>> GetAllArticlesAsync();
    Task<IEnumerable<Article>> GetPublishedArticlesAsync();
}