using System.Drawing;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    private void ParseImgTag(string tag, out bool isBase64, out string base64, out string format, out string link, out bool isOuterLink)
    {
        format = "";
        link = "";
        base64 = "";
        isOuterLink = false;
        isBase64 = tag.Substring(10, 4) == "data";
        if (isBase64)
        {
            var base64StartIndex = tag.IndexOf(",", StringComparison.Ordinal);
            base64 = tag.Substring(
                startIndex: base64StartIndex + 1,
                length: tag.Length - base64StartIndex - 3);
            format = tag.Substring(
                startIndex: 21, 
                length: tag.IndexOf(';') - 21);
            return;
        }

        int srcOffset = tag.IndexOf("src", StringComparison.Ordinal) - 5;

        int possibleQueryIndex = tag.IndexOf('?', 11 + srcOffset);
        int closingQuoteIndex = tag.IndexOf('"', 11+ srcOffset);
        int linkEndingIndex = possibleQueryIndex > 0 && possibleQueryIndex < closingQuoteIndex
            ? possibleQueryIndex
            : closingQuoteIndex;
        link = tag.Substring(10 + srcOffset, linkEndingIndex - 10 - srcOffset);
        isOuterLink = link.Substring(0, _configuration["Azure:ContainerLink"].Length) != _configuration["Azure:ContainerLink"];
    }

    private async Task<string> UploadImages(string body)
    {
        int previousTagIndex = 0;
        while (true)
        {
            var tagIndex = body.IndexOf("<img", previousTagIndex, StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }

            previousTagIndex = tagIndex + 1;
            
            var closingQuoteIndex = body.IndexOf('>', tagIndex);
            var tag = body.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);

            ParseImgTag(tag, out bool isBase64, out var base64, out var format, out var link, out var isOuterLink);
            if (isBase64)
            {
                var fileName = await _imageService.UploadFromBase64Async(
                    base64: base64,
                    folder: "articles",
                    imageFormat: format);
                var newLink = _configuration["Azure:ContainerLink"] + "/" + _configuration["Azure:ContainerName"] + "/" + fileName;
                
                body = body.Remove(
                    startIndex: tagIndex + 10, // 10 for <img src=" length
                    count: closingQuoteIndex - (tagIndex + 10));
                body = body.Insert(tagIndex + 10, newLink + '"');
            }

            if (isOuterLink)
            {
                //avoid writing
                body = body.Remove(
                    startIndex: tagIndex ,
                    count: closingQuoteIndex - tagIndex);
                body = body.Insert(tagIndex, "<img src=\"" +link + '"');
            }
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
            var tagIndex = body.IndexOf("<img", StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }
            var closingQuoteIndex = body.IndexOf('>', tagIndex);
            var tag = body.Substring(tagIndex, closingQuoteIndex - tagIndex + 1);

            ParseImgTag(tag, out _, out _, out _, out var link, out var isOuterLink);

            if (!isOuterLink)
            {
                int nameIndex = link.LastIndexOf('/');
                var name = link.Substring(nameIndex + 1);
                await _imageService.DeleteAsync(
                    imageName: name,
                    folder: "articles");
            }

            body = body.Remove(
                startIndex: tagIndex ,
                count: closingQuoteIndex - tagIndex + 1);        
        }

        return body;
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