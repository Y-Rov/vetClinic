using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IViewModelMapper<CreateCommentViewModel, Comment> _createMapper;
    private readonly IViewModelMapper<UpdateCommentViewModel, Comment> _updateMapper;
    private readonly IViewModelMapper<Comment, ReadCommentViewModel> _readMapper;
    private readonly IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>> _readEnumMapper;
    private readonly UserManager<User> _userManager;

    public CommentsController(
        ICommentService commentService,
        IViewModelMapper<CreateCommentViewModel, Comment> createMapper,
        IViewModelMapper<UpdateCommentViewModel, Comment> updateMapper,
        IViewModelMapper<Comment, ReadCommentViewModel> readMapper,
        IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>> readEnumMapper,
        UserManager<User> userManager)
    {
        _commentService = commentService;
        _createMapper = createMapper;
        _updateMapper = updateMapper;
        _readMapper = readMapper;
        _readEnumMapper = readEnumMapper;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ReadCommentViewModel>> GetAsync()
    {
        var articles = await _commentService.GetAllCommentsAsync();
        var viewModels = _readEnumMapper.Map(articles);
        return viewModels;
    }
    
    [HttpGet("article/{id:int:min(1)}")]
    public async Task<IEnumerable<ReadCommentViewModel>> GetAllArticleCommentsAsync([FromRoute]int id)
    {
        var comments = await _commentService.GetAllArticleCommentsAsync(id);
        var viewModels = _readEnumMapper.Map(comments);
        return viewModels;
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ReadCommentViewModel> GetByIdAsync([FromRoute]int id)
    {
        var comment = await _commentService.GetByIdAsync(id);
        var viewModel = _readMapper.Map(comment);
        return viewModel;
    }
    
    //[Authorize(Roles = "Client")]
    [HttpPost]
    public async Task CreateAsync([FromBody] CreateCommentViewModel viewModel)
    {
        var newArticle = _createMapper.Map(viewModel);
        await _commentService.CreateCommentAsync(newArticle);
    }

    //[Authorize(Roles = "Client")]
    [HttpDelete("{id:int:min(1)}")]
    public async Task DeleteAsync([FromRoute] int id)
    {
        var requestUser = await _userManager.GetUserAsync(HttpContext.User);
        await _commentService.DeleteCommentAsync(id, requestUser);
    }
    
    //[Authorize(Roles = "Client")]
    [HttpPut]
    public async Task UpdateAsync([FromBody] UpdateCommentViewModel viewModel)
    {
        var requestUser = await _userManager.GetUserAsync(HttpContext.User);
        var updatedArticle = _updateMapper.Map(viewModel);
        await _commentService.UpdateCommentAsync(updatedArticle, requestUser);
    }
}