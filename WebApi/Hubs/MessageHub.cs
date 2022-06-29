using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.AutoMapper.Interface;

namespace WebApi.Hubs;

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

    public async Task SendMessage(MessageSendViewModel message)
    {
        bool chatRoomExists = await _chatRoomService.ExistsAsync(message.ChatRoomId);
        if (chatRoomExists)
        {
            var messageMap = _sendMessageMapper.Map(message);
            int senderId;
            var parseSucceeded = Int32.TryParse(Context.UserIdentifier, out senderId);
            if (parseSucceeded)
            {
                messageMap.SenderId = senderId;
                await _messageService.CreateAsync(messageMap);
                var messageGetMap = _getMessageMapper.Map(messageMap);
                await Clients.Group(messageMap.ChatRoomId.ToString())
                    .SendAsync("getMessage", messageGetMap);
            }
        }
    }
}