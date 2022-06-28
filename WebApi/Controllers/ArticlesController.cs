using Application.Services;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.ArticleMappers;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/articles")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>>
        _enumerableViewModelMapper;

    private readonly IArticleService _articleService;
    private readonly IViewModelMapper<CreateArticleViewModel, Article>  _createMapper;
    private readonly IViewModelMapper<UpdateArticleViewModel, Article> _updateMapper;
    private readonly IViewModelMapper<Article, ReadArticleViewModel> _readMapper;

    public ArticlesController(
        IArticleService articleService,
        IViewModelMapper<CreateArticleViewModel, Article> createMapper,
        IViewModelMapper<UpdateArticleViewModel, Article> updateMapper,
        IViewModelMapper<Article, ReadArticleViewModel> readMapper,
        IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>> enumerableViewModelMapper)
    {
        _enumerableViewModelMapper = enumerableViewModelMapper;
        _articleService = articleService;
        _createMapper = createMapper;
        _updateMapper = updateMapper;
        _readMapper = readMapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ReadArticleViewModel>> GetAsync()
    {
        var articles = await _articleService.GetAllArticlesAsync();
        var viewModels = _enumerableViewModelMapper.Map(articles);
        return viewModels;
    }

    [HttpGet("published")]
    public async Task<IEnumerable<ReadArticleViewModel>> GetPublishedAsync()
    {
        var articles = await _articleService.GetPublishedArticlesAsync();
        var viewModels = _enumerableViewModelMapper.Map(articles);
        return viewModels;
    }

        
    [HttpGet("{id:int:min(1)}")]
    public async Task<ReadArticleViewModel> GetByIdAsync([FromRoute]int id)
    {
        var article = await _articleService.GetByIdAsync(id);
        var viewModel = _readMapper.Map(article);
        return viewModel;
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task CreateAsync([FromBody] CreateArticleViewModel viewModel)
    {
        var newArticle = _createMapper.Map(viewModel);
        await _articleService.CreateArticleAsync(newArticle);
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task DeleteAsync([FromRoute] int id)
    {
        await _articleService.DeleteArticleAsync(id);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task UpdateAsync([FromBody] UpdateArticleViewModel viewModel)
    {
        var updatedArticle = _updateMapper.Map(viewModel);
        await _articleService.UpdateArticleAsync(updatedArticle);
    }
}