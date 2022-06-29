using Core.Entities;

namespace Core.Interfaces.Services;

public interface IMessageService
{
    Task<IEnumerable<Message>> LoadMessagesInChatRoomAsync(int chatRoomId, int skip, int take);
    Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId);
    Task<IEnumerable<Message>> GetUnrepliedQuestionsAsync();
    Task CreateAsync(Message message);
}