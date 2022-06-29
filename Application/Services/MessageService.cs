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
    
    public async Task<IEnumerable<Message>> LoadMessagesInChatRoomAsync(int chatRoomId, int skip, int take)
    {
        if (!await _chatRoomRepository.ExistsAsync(chatRoomId))
            throw new NotFoundException($"Chatroom with id {chatRoomId} does not exist");
        
        var messagesToRead = await _messageRepository.Query(
                filter: m => m.ChatRoomId == chatRoomId,
                orderBy: q => q.OrderByDescending(m => m.SentAt),
                include: q => q.Include(m => m.Sender),
                skip: skip,
                take: take
            ).ToListAsync();

        messagesToRead.ForEach(m => m.IsRead = true);
        await _messageRepository.SaveChangesAsync();

        return messagesToRead;
    }

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
            throw new NotFoundException($"User with id {userId} does not exist");
        
        var roomsIds = await _userChatRoomRepository
            .Query(filter: ur => ur.UserId == userId)
            .Select(ur => ur.UserId)
            .ToListAsync();
        
        return await _messageRepository.Query(
            filter: m =>  roomsIds.Contains(m.ChatRoomId)  && !m.IsRead,
            asNoTracking: true
            ).ToListAsync();
    }

    public async Task CreateAsync(Message message)
    {
        if (!await _chatRoomRepository.ExistsAsync(message.ChatRoomId))
            throw new NotFoundException($"Chat room with id {message.ChatRoomId} does not exist");

        await _messageRepository.InsertAsync(message);
    }
}