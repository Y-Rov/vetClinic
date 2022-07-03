using Core.Entities;

namespace Core.Interfaces.Services;

public interface IChatRoomService
{
    Task CreateAsync(ChatRoom chatRoom);
    Task<IEnumerable<User>> GetParticipantsAsync(int chatRoomId);
    Task AddMemberAsync(int roomId, int userId);
    Task KickMemberAsync(int roomId, int userId);
    Task<ChatRoom> EnsurePrivateRoomCreatedAsync(int memberId1, int memberId2);
    Task<UserChatRoom?> GetUserChatRoomAsync(int userId, int chatRoomId);
    Task<bool> ExistsAsync(int id);
}