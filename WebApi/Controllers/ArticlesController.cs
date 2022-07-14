using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ArticleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/articles")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;
    private readonly IViewModelMapper<CreateArticleViewModel, Article>  _createMapper;
    private readonly IViewModelMapper<UpdateArticleViewModel, Article> _updateMapper;
    private readonly IViewModelMapper<Article, ReadArticleViewModel> _readMapper;
    private readonly IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>> _readPagedMapper;
    private readonly UserManager<User> _userManager;
    private readonly IImageService _imageService;


    public ArticlesController(
        IArticleService articleService,
        IViewModelMapper<CreateArticleViewModel, Article> createMapper,
        IViewModelMapper<UpdateArticleViewModel, Article> updateMapper,
        IViewModelMapper<Article, ReadArticleViewModel> readMapper,
        IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>> readPagedMapper,
        UserManager<User> userManager, IImageService imageService)
    {
        _articleService = articleService;
        _createMapper = createMapper;
        _updateMapper = updateMapper;
        _readMapper = readMapper;
        _readPagedMapper = readPagedMapper;
        _userManager = userManager;
        _imageService = imageService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<PagedReadViewModel<ReadArticleViewModel>> GetAsync([FromQuery] ArticleParameters parameters)
    {
        var articles = await _articleService.GetArticlesAsync(parameters);
        var viewModels = _readPagedMapper.Map(articles);
        return viewModels;
    }    
    
    [HttpGet("published")]
    public async Task<PagedReadViewModel<ReadArticleViewModel>> GetPublishedAsync([FromQuery] ArticleParameters parameters)
    {
        var articles = await _articleService.GetPublishedArticlesAsync(parameters);
        var viewModels = _readPagedMapper.Map(articles);
        return viewModels;
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ReadArticleViewModel> GetByIdAsync([FromRoute]int id)
    {
        var article = await _articleService.GetByIdAsync(id);
        var viewModel = _readMapper.Map(article);
        return viewModel;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task CreateAsync([FromBody] CreateArticleViewModel viewModel)
    {
        var newArticle = _createMapper.Map(viewModel);
        await _articleService.CreateArticleAsync(newArticle);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task DeleteAsync([FromRoute] int id)
    {
        await _articleService.DeleteArticleAsync(id);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task UpdateAsync([FromBody] UpdateArticleViewModel viewModel)
    {
        var updatedArticle = _updateMapper.Map(viewModel);
        await _articleService.UpdateArticleAsync(updatedArticle);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("upload")]
    public async Task<ImageViewModel> UploadImage(IFormFile file)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var link = await _imageService.UploadImageAsync(file, user.Id);
        return new ImageViewModel()
        {
            ImageUrl = link
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("discard")]
    public async Task DiscardEditing()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _imageService.DiscardCachedImagesAsync(user.Id);
    }
}