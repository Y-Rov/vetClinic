using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.AutoMapper.Interface;

namespace WebApi.SignalR.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IChatRoomService _chatRoomService;
    private readonly IViewModelMapper<MessageSendViewModel, Message> _sendMessageMapper;
    private readonly IViewModelMapper<Message, MessageGetViewModel> _getMessageMapper;
    public MessageHub(
        IMessageService messageService, 
        IChatRoomService chatRoomService, 
        IViewModelMapper<MessageSendViewModel, Message> sendMessageMapper,
        IViewModelMapper<Message, MessageGetViewModel> getMessageMapper)
    {
        _messageService = messageService;
        _chatRoomService = chatRoomService;
        _sendMessageMapper = sendMessageMapper;
        _getMessageMapper = getMessageMapper;
    }

    public async Task SendPrivateMessage(MessageSendViewModel message)
    {
        if (TryParseUserId(out int senderId))
        {
            await _chatRoomService.EnsurePrivateRoomCreatedAsync(senderId, message.ReceiverId);

            var messageMap = _sendMessageMapper.Map(message);
            messageMap.SenderId = senderId;
            messageMap.SentAt = DateTime.Now;
            await _messageService.CreateAsync(messageMap);
            
            var messageGetMap = _getMessageMapper.Map(messageMap);
            await Clients.User(message.ReceiverId.ToString())
                .SendAsync("getMessage", messageGetMap);
        }
    }

    public async Task ReadMessage(int messageId)
    {
        if (TryParseUserId(out int readerId))
        {
            await _messageService.ReadMessageAsync(readerId, messageId);
        }
    }

    private bool TryParseUserId(out int userId)
    {
        return Int32.TryParse(Context.UserIdentifier, out userId);
    }
}