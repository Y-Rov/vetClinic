using System.Drawing;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILoggerManager _loggerManager;
    private readonly IConfiguration _configuration;
    private readonly IImageService _imageService;

    public ArticleService(
        IArticleRepository articleRepository,
        ILoggerManager loggerManager,
        IConfiguration configuration,
        IImageService imageService)
    {
        _articleRepository = articleRepository;
        _loggerManager = loggerManager;
        _configuration = configuration;
        _imageService = imageService;
    }

    private async Task<string> UploadImages(string body)
    {
        while (true)
        {
            var tagIndex = body.IndexOf("<img src=\"data:image/", StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }
            
            var format = body.Substring(
                startIndex: tagIndex + 21, // 21 for <img src="data:image/ length
                length: body.IndexOf(';', tagIndex) - tagIndex - 21);
            
            var closingQuoteIndex = body.IndexOf("\">", tagIndex, StringComparison.Ordinal);
            var base64StartIndex = body.IndexOf(",", tagIndex, StringComparison.Ordinal);
            var base64Str = body.Substring(
                startIndex: base64StartIndex + 1, //+1 for separating comma: ...base64,iVBORw0KG...
                length: closingQuoteIndex - base64StartIndex - 1); //-1 for the closing " of the tag

            var fileName = await _imageService.UploadFromBase64Async(
                base64: base64Str,
                folder: "articles",
                imageFormat: format);

            var link = _configuration["Azure:ContainerLink"] + "/" + _configuration["Azure:ContainerName"] + "/" + fileName;

            body = body.Remove(
                startIndex: tagIndex + 10, // 10 for <img src=" length
                count: closingQuoteIndex - (tagIndex + 10));
            body = body.Insert(tagIndex + 10, link);
        }

        return body;
    }
    
    private Image LoadImage(string base64String)
    {
        var bytes = Convert.FromBase64String(base64String);

        var ms = new MemoryStream(bytes);
        var image = Image.FromStream(ms);
        return image;
    }
    
    public async Task CreateArticleAsync(Article article)
    {
        article.Body = await UploadImages(article.Body!);
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
        var updatingArticle = await GetByIdAsync(article.Id);
        updatingArticle.Title = article.Title;
        updatingArticle.Body = await UploadImages(article.Body);
        updatingArticle.Published = article.Published;
        updatingArticle.Edited = true;

        await _articleRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated article with id {article.Id}");
    }

    private async Task<string> DeleteImages(string body)
    {
        while (true)
        {
            var nameIndex = body.IndexOf("<img src=\"http", StringComparison.Ordinal);
            if (nameIndex == -1)
            {
                return body;
            }

            int end = body.IndexOf("\">", nameIndex, StringComparison.Ordinal);
            int start = body.LastIndexOf("/", end, StringComparison.Ordinal) + 1;
            var fileName = body.Substring(start, end - start);

            await _imageService.DeleteAsync( 
                imageName: fileName,
                folder: "articles");

            body = body.Remove(nameIndex , end - nameIndex);
        }
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        var articleToRemove = await GetByIdAsync(articleId);

        articleToRemove.Body = await DeleteImages(articleToRemove.Body);
        
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