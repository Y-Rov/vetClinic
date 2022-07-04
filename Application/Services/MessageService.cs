using Core.Entities;
using Core.Exceptions;
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

    public MessageService(
        IMessageRepository messageRepository,
        IUserChatRoomRepository userChatRoomRepository,
        IChatRoomRepository chatRoomRepository,
        IUserService userService)
    {
        _messageRepository = messageRepository;
        _userChatRoomRepository = userChatRoomRepository;
        _chatRoomRepository = chatRoomRepository;
        _userService = userService;
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        return await _messageRepository.GetFirstOrDefaultAsync(
            filter: m => m.Id == id);
    }

    public async Task<IEnumerable<Message>> GetMessagesInChatRoomAsync(
        int readerId, int chatRoomId, int skip, int take)
    {
        if (!await _chatRoomRepository.ExistsAsync(chatRoomId))
            throw new NotFoundException($"Chatroom with id {chatRoomId} does not exist");

        var messagesToRead = await _messageRepository.QueryAsync(
            filter: m => m.ChatRoomId == chatRoomId,
            orderBy: q => q.OrderByDescending(m => m.SentAt),
            include: q => q.Include(m => m.Sender!),
            skip: skip,
            take: take
        );

        await _messageRepository.SaveChangesAsync();

        return messagesToRead;
    }

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
    {
        return await _messageRepository.GetUnreadMessagesAsync(userId);
    }

    public async Task CreateAsync(Message message)
    {
        if (!await _chatRoomRepository.ExistsAsync(message.ChatRoomId))
            throw new NotFoundException($"Chat room with id {message.ChatRoomId} does not exist");

        await _messageRepository.InsertAsync(message);
    }

    public async Task ReadMessageAsync(int readerId, int messageId)
    {
        var message = await GetByIdAsync(messageId);
        
        if (message is null)
            throw new NotFoundException($"Message with id {messageId} does not exist");

        var userChatRoom = await _userChatRoomRepository.GetFirstOrDefaultAsync(
            filter: ur => ur.UserId == readerId && ur.ChatRoomId == message.ChatRoomId,
            include: q => q.Include(ur => ur.LastReadMessage)!);

        if (userChatRoom!.LastReadMessage is null || userChatRoom.LastReadMessage.SentAt < message.SentAt)
        {
            userChatRoom.LastReadMessageId = message.Id;
        }
    }
}