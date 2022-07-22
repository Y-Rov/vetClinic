using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserChatRoomRepository _userChatRoomRepository;
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserService _userService;
    private readonly ILoggerManager _loggerManager;

    public MessageService(
        IMessageRepository messageRepository,
        IUserChatRoomRepository userChatRoomRepository,
        IChatRoomRepository chatRoomRepository,
        IUserService userService,
        ILoggerManager loggerManager)
    {
        _messageRepository = messageRepository;
        _userChatRoomRepository = userChatRoomRepository;
        _chatRoomRepository = chatRoomRepository;
        _userService = userService;
        _loggerManager = loggerManager;
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        var message = await _messageRepository.GetFirstOrDefaultAsync(
            filter: m => m.Id == id);
        
        if (message is null)
            _loggerManager.LogInfo($"No message with id {id} was not found");
        
        _loggerManager.LogInfo($"Returned message with id {id}");
        
        return message;
    }

    public async Task<IEnumerable<Message>> GetMessagesInChatRoomAsync(
        int chatRoomId, int skip, int take)
    {
        if (!await _chatRoomRepository.ExistsAsync(chatRoomId))
        {
            _loggerManager.LogWarn($"Chatroom with id {chatRoomId} does not exist");
            throw new NotFoundException($"Chatroom with id {chatRoomId} does not exist");
        }
        
        var messagesToRead = await _messageRepository.QueryAsync(
            filter: m => m.ChatRoomId == chatRoomId,
            orderBy: q => q.OrderByDescending(m => m.SentAt),
            include: q => q.Include(m => m.Sender!),
            skip: skip,
            take: take,
            asNoTracking: true
        );
        
        _loggerManager.LogInfo($"Returned messages in chat room {chatRoomId}");

        return messagesToRead;
    }

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"User with id {userId} was not found");
            
        var userChatRooms = await _userChatRoomRepository.QueryAsync(
            include: q => 
                q.Include(ur => ur.LastReadMessage!)
                    .Include(ur => ur.ChatRoom)
                    .ThenInclude(cr => cr.Messages),
            filter: ur => ur.UserId == userId
        );

        var unreadMessages = userChatRooms.SelectMany(ur => ur.ChatRoom.Messages.Where(m =>
        {
            var lastReadDateTime = ur.LastReadMessage?.SentAt ?? DateTime.MinValue;
            return (m.SentAt > lastReadDateTime && m.SenderId != userId);
        }));
        
        _loggerManager.LogInfo($"Returned unread messages for user {userId}");
        
        return unreadMessages;
    }

    public async Task CreateAsync(Message message)
    {
        if (!await _chatRoomRepository.ExistsAsync(message.ChatRoomId))
        {
            _loggerManager.LogWarn($"Chat room with id {message.ChatRoomId} does not exist");
            throw new NotFoundException($"Chat room with id {message.ChatRoomId} does not exist");
        }

        await _messageRepository.InsertAsync(message);
        await _messageRepository.SaveChangesAsync();
        
        _loggerManager.LogInfo($"Created message #{message.Id} from user #{message.SenderId} in chatRoom #{message.ChatRoomId}");
    }

    public async Task ReadAsync(int readerId, int messageId)
    {
        var message = await GetByIdAsync(messageId);

        if (message is null)
        {
            _loggerManager.LogWarn($"Message with id {messageId} does not exist");
            throw new NotFoundException($"Message with id {messageId} does not exist");
        }

        var userChatRoom = await _userChatRoomRepository.GetFirstOrDefaultAsync(
            filter: ur => ur.UserId == readerId && ur.ChatRoomId == message.ChatRoomId,
            include: q => q.Include(ur => ur.LastReadMessage)!);

        if (userChatRoom!.LastReadMessage is null || userChatRoom.LastReadMessage.SentAt < message.SentAt)
        {
            userChatRoom.LastReadMessageId = message.Id;
            await _messageRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Updated last read message for user #{readerId} #{message.ChatRoomId}");
        }
    }
}