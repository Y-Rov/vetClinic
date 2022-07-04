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
    private readonly int _userId;
    private readonly IMessageService _messageService;
    private readonly IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> 
        _enumMessageMapper;

    public MessagesController(
        IMessageService messageService, 
        IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> enumMessageMapper)
    {
        Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out _userId);
        _messageService = messageService;
        _enumMessageMapper = enumMessageMapper;
    }
    
    [HttpGet("{chatRoomId:int:min(1)}/{skip:int:min(0)}/{take:int:min(1)}")]
    public async Task<IEnumerable<MessageGetViewModel>> GetMessagesInChatRoom(int chatRoomId, int skip, int take)
    {
        var messages = await _messageService
            .GetMessagesInChatRoomAsync(_userId, chatRoomId, skip, take);
        return _enumMessageMapper.Map(messages);
    }
    
    [HttpGet]
    public async Task<IEnumerable<MessageGetViewModel>> GetUnreadMessages()
    {
        var messages = await _messageService.GetUnreadMessagesAsync(_userId);
        return _enumMessageMapper.Map(messages);
    }
}