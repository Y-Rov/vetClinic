using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services;

public interface IArticleService
{
    Task CreateArticleAsync(Article article);
    Task UpdateArticleAsync(Article article);
    Task DeleteArticleAsync(int articleId);
    Task<Article> GetByIdAsync(int articleId);
    Task<PagedList<Article>> GetArticlesAsync(ArticleParameters parameters);
    Task<PagedList<Article>> GetPublishedArticlesAsync(ArticleParameters parameters);
}