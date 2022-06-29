using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserChatRoomRepository _userChatRoomRepository;
    
    public MessageService(
        IMessageRepository messageRepository,
        IUserChatRoomRepository userChatRoomRepository)
    {
        _messageRepository = messageRepository;
        _userChatRoomRepository = userChatRoomRepository;
    }
    
    public async Task<IEnumerable<Message>> LoadMessagesInChatRoomAsync(int chatRoomId, int skip, int take)
    {
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
        var roomsIds = await _userChatRoomRepository
            .Query(filter: ur => ur.UserId == userId)
            .Select(ur => ur.UserId)
            .ToListAsync();
        
        return await _messageRepository.Query(
            filter: m =>  roomsIds.Contains(m.ChatRoomId)  && !m.IsRead
            ).ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetUnrepliedQuestionsAsync()
    {
        return await _messageRepository.Query(
            filter: m => m.ChatRoomId == 1
            ).ToListAsync();
    }

    public async Task CreateAsync(Message message)
    {
        await _messageRepository.InsertAsync(message);
    }
}