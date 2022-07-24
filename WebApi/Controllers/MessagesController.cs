using System.Runtime.CompilerServices;
using System.Security.Claims;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.AutoMapper.Interface;
using WebApi.SignalR.Hubs;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IChatRoomService _chatRoomService;
    private readonly UserManager<User> _userManager;
    private readonly IHubContext<MessageHub> _messageHubContext;
    private readonly IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> 
        _enumMessageMapper;
    private readonly IViewModelMapper<Message, MessageGetViewModel> _messageGetMapper;
    private readonly IViewModelMapper<MessageSendViewModel, Message> _messageSendMapper;

    public MessagesController(
        IMessageService messageService, 
        IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>> enumMessageMapper, 
        IViewModelMapper<Message, MessageGetViewModel> messageGetMapper,
        UserManager<User> userManager, 
        IHubContext<MessageHub> messageHubContext, 
        IChatRoomService chatRoomService, 
        IViewModelMapper<MessageSendViewModel, Message> messageSendMapper)
    {
        _messageService = messageService;
        _enumMessageMapper = enumMessageMapper;
        _userManager = userManager;
        _messageHubContext = messageHubContext;
        _chatRoomService = chatRoomService;
        _messageGetMapper = messageGetMapper;
        _messageSendMapper = messageSendMapper;
    }
    
    [HttpGet("{chatRoomId:int:min(1)}/{skip:int:min(0)}/{take:int:min(1)}")]
    public async Task<IEnumerable<MessageGetViewModel>> GetMessagesInChatRoomAsync(int chatRoomId, int skip, int take)
    {
        var messages = await _messageService
            .GetMessagesInChatRoomAsync(chatRoomId, skip, take);
        return _enumMessageMapper.Map(messages);
    }
    
    [HttpGet("unread")]
    public async Task<IEnumerable<MessageGetViewModel>> GetUnreadMessagesAsync()
    {
        var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

        var messages = await _messageService.GetUnreadMessagesAsync(userId);
        return _enumMessageMapper.Map(messages);
    }

    [HttpPost]
    public async Task SendPrivateMessageAsync(MessageSendViewModel message)
    {
        var senderId = (await _userManager.GetUserAsync(User)).Id;
        
        var chatRoom = await _chatRoomService.EnsurePrivateRoomCreatedAsync(senderId, message.ReceiverId);

        var messageMap = _messageSendMapper.Map(message);
        messageMap.ChatRoomId = chatRoom.Id;
        messageMap.SenderId = senderId;
        messageMap.SentAt = DateTime.Now;
        await _messageService.CreateAsync(messageMap);
            
        var messageGetMap = _messageGetMapper.Map(messageMap);
        await _messageHubContext.Clients.User(message.ReceiverId.ToString())
            .SendAsync("getMessage", messageGetMap);
    }

    [HttpPost("read/{messageId:int:min(1)}")]
    public async Task ReadMessageAsync([FromRoute] int messageId)
    {
        var readerId = (await _userManager.GetUserAsync(User)).Id;
        await _messageService.ReadAsync(readerId, messageId);
    }
    
}