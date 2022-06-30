using System.Drawing;
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
    private readonly IArticleImageManager _imageManager;

    public ArticleService(
        IArticleRepository articleRepository,
        ILoggerManager loggerManager,
        IArticleImageManager imageManager)
    {
        _articleRepository = articleRepository;
        _loggerManager = loggerManager;
        _imageManager = imageManager;
    }

    private async Task<string> UploadImages(string body)
    {
        while (true)
        {
            var index = body.IndexOf("<img src=\"data:image/", StringComparison.Ordinal);
            if (index == -1)
            {
                break;
            }
            var tagLength = "<img src=\"data:image/".Length;

            int formatLength = 0;
            for (int i = index + tagLength; i < index + tagLength + 10; i++)
            {
                if (body[i] == ';')
                {
                    formatLength = i - (index + tagLength);
                    break;
                }
            }

            var format = body.Substring(index + tagLength, formatLength);

            var closingQuoteIndex = body.IndexOf("\">", index, StringComparison.Ordinal);

            var base64StrStartIndex = body.IndexOf(",", index + tagLength, StringComparison.Ordinal);

            var base64Str = body.Substring(
                startIndex: base64StrStartIndex + 1, //+1 for separating comma: png;base64-->,<--iVBORw0KG
                length: closingQuoteIndex - base64StrStartIndex - 1); //-1 for the closing " of the tag

            var image = LoadImage(base64Str);
            var fileName = await _imageManager.UploadAsync(image, format);
            var link = "http://127.0.0.1:10000/devstoreaccount1/vet-clinic/" + fileName;
            body = body.Remove(
                startIndex: index + "<img src=\"".Length,
                count: "data:image/".Length + formatLength + ";base64,".Length + base64Str.Length);
            
            body = body.Insert(index + "<img src=\"".Length, link);
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
        updatingArticle.Body = article.Body;
        updatingArticle.Published = article.Published;
        updatingArticle.Edited = true;

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