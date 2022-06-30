using System.Drawing;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            var tagIndex = body.IndexOf("<img src=\"data:image/", StringComparison.Ordinal);
            if (tagIndex == -1)
            {
                break;
            }
            
            var tagLength = "<img src=\"data:image/".Length;
            var formatIndex = tagIndex + "<img src=\"data:image/".Length;

            int formatLength = body.IndexOf(';', tagIndex) - formatIndex;
            var format = body.Substring(formatIndex, formatLength);
            
            var closingQuoteIndex = body.IndexOf("\">", tagIndex, StringComparison.Ordinal);
            var base64StartIndex = body.IndexOf(",", formatIndex, StringComparison.Ordinal);
            var base64Str = body.Substring(
                startIndex: base64StartIndex + 1, //+1 for separating comma: png;base64-->,<--iVBORw0KG
                length: closingQuoteIndex - base64StartIndex - 1); //-1 for the closing " of the tag

            var image = LoadImage(base64Str);
            var fileName = await _imageManager.UploadAsync(image, format);
            
            var link = "http://127.0.0.1:10000/devstoreaccount1/vet-clinic/" + fileName;
            body = body.Remove(
                startIndex: tagIndex + "<img src=\"".Length,
                count: "data:image/".Length + formatLength + ";base64,".Length + base64Str.Length);
            body = body.Insert(tagIndex + "<img src=\"".Length, link);
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

    private async Task DeleteImages(string body)
    {
        while (true)
        {
            var nameIndex = body.IndexOf("<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles",
                StringComparison.Ordinal);
            if (nameIndex == -1)
            {
                return;
            }

            int start = nameIndex + "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/".Length;
            int end = body.IndexOf("\">", nameIndex, StringComparison.Ordinal);
            var fileName = body.Substring(start, end - start);

            await _imageManager.DeleteAsync(fileName);

            body = body.Remove( nameIndex , end - nameIndex);
        }
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        var articleToRemove = await GetByIdAsync(articleId);

        await DeleteImages(articleToRemove.Body);
        
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