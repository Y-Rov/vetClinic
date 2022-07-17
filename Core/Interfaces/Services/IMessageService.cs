using Core.Entities;

namespace Core.Interfaces.Services;

public interface IMessageService
{
    Task<Message?> GetByIdAsync(int id);
    Task<IEnumerable<Message>> GetMessagesInChatRoomAsync(int chatRoomId, int skip, int take);
    Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId);
    Task CreateAsync(Message message);
    Task ReadMessageAsync(int readerId, int messageId);
}