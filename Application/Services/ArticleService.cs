﻿using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using System.Linq.Expressions;

namespace Application.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILoggerManager _loggerManager;
    private readonly IImageService _imageService;

    public ArticleService(
        IArticleRepository articleRepository,
        ILoggerManager loggerManager,
        IImageService imageService
        )
    {
        _articleRepository = articleRepository;
        _loggerManager = loggerManager;
        _imageService = imageService;
    }

    public async Task CreateArticleAsync(Article article)
    {
        article.Body = _imageService.TrimArticleImages(article.Body!);
        await _imageService.ClearUnusedImagesAsync(article.Body, article.AuthorId);
        await _articleRepository.InsertAsync(article);
        await _articleRepository.SaveChangesAsync();

        _loggerManager.LogInfo($"Created new article with title {article.Title}");
    }
    
    public async Task UpdateArticleAsync(Article article)
    {
        var updatingArticle = await GetByIdAsync(article.Id);
        updatingArticle.Title = article.Title;
        
        await _imageService.ClearUnusedImagesAsync(article.Body!, updatingArticle.AuthorId);
        await _imageService.ClearOutdatedImagesAsync(article.Body!, updatingArticle.Body!);
        updatingArticle.Body = _imageService.TrimArticleImages(article.Body!);
        
        updatingArticle.Published = article.Published;
        updatingArticle.Edited = true;

        await _articleRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated article with id {article.Id}");
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        var articleToRemove = await GetByIdAsync(articleId);
        articleToRemove.Body = await _imageService.DeleteImagesAsync(articleToRemove.Body!);

        _articleRepository.Delete(articleToRemove);
        await _articleRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted article with id {articleId}");
    }

    public async Task<Article> GetByIdAsync(int articleId)
    {
        var article = await _articleRepository.GetById(articleId, includeProperties: "Author");
        if (article is null)
        {
            _loggerManager.LogWarn($"Article with id {articleId} does not exist");
            throw new NotFoundException($"Article with id {articleId} does not exist");
        }
        
        _loggerManager.LogInfo($"Found article with id {articleId}");
        return article;
    }

    public async Task<PagedList<Article>> GetArticlesAsync(ArticleParameters parameters)
    {
        var filterQuery = GetFilterQuery(parameters.FilterParam, false);
        var orderByQuery = GetOrderByQuery(parameters.OrderByParam);

        var articles = await _articleRepository.GetPaged(
            parameters: parameters,
            filter: filterQuery,
            orderBy: orderByQuery,
            includeProperties: "Author"
        );
        _loggerManager.LogInfo("Found all articles");
        return articles;
    }
    
    public async Task<PagedList<Article>> GetPublishedArticlesAsync(ArticleParameters parameters)
    {
        var filterQuery = GetFilterQuery(parameters.FilterParam, true);
        var orderByQuery = GetOrderByQuery(parameters.OrderByParam);

        var articles = await _articleRepository.GetPaged(
            parameters: parameters,
            filter: filterQuery,
            orderBy: orderByQuery,
            includeProperties: "Author"
        );
        _loggerManager.LogInfo("Found all articles");
        return articles;
    }

    private static Expression<Func<Article, bool>>? GetFilterQuery(string? filterParam, bool isPublished)
    {
        Expression<Func<Article, bool>>? filterQuery = null;
        
        if (string.IsNullOrEmpty(filterParam))
        {
            if(isPublished)
                filterQuery = art => art.Published;
            
            return filterQuery;
        }
        
        var formattedFilter = filterParam.Trim().ToLower();
        
        if (isPublished)
        {
            filterQuery = art => (art.Title!.ToLower().Contains(formattedFilter)
                                  || art.Body!.ToLower().Contains(formattedFilter)) && art.Published;
        }
        else
        {
            filterQuery = art => art.Title!.ToLower().Contains(formattedFilter)
                                 || art.Body!.ToLower().Contains(formattedFilter);
        }
        return filterQuery;
    }

    private static Func<IQueryable<Article>, IOrderedQueryable<Article>>? GetOrderByQuery(string? orderBy) => orderBy switch
    {
        "Title" => query => query.OrderBy(article => article.Title),
        "Creation Time" => query => query.OrderBy(article => article.CreatedAt),
        _ => null
    };
}