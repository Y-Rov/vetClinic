using System.Runtime.CompilerServices;
using System.Security.Claims;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly UserManager<User> _userManager;
    private readonly IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> 
        _enumMessageMapper;

    public MessagesController(
        IMessageService messageService, 
        IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> enumMessageMapper, UserManager<User> userManager)
    {
        _messageService = messageService;
        _enumMessageMapper = enumMessageMapper;
        _userManager = userManager;
    }
    
    [HttpGet("{chatRoomId:int:min(1)}/{skip:int:min(0)}/{take:int:min(1)}")]
    public async Task<IEnumerable<MessageGetViewModel>> GetMessagesInChatRoom(int chatRoomId, int skip, int take)
    {
        var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
        
        var messages = await _messageService
            .GetMessagesInChatRoomAsync(userId, chatRoomId, skip, take);
        return _enumMessageMapper.Map(messages);
    }
    
    [HttpGet("unread")]
    public async Task<IEnumerable<MessageGetViewModel>> GetUnreadMessages()
    {
        var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

        var messages = await _messageService.GetUnreadMessagesAsync(userId);
        return _enumMessageMapper.Map(messages);
    }
}