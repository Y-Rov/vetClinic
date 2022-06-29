using Core.Entities;

namespace Core.Interfaces.Services;

public interface IChatRoomService
{
    Task CreateAsync(ChatRoom chatRoom);
    Task<IEnumerable<User>> GetParticipantsAsync(int chatRoomId);
    Task AddMemberAsync(int roomId, int userId);
    Task KickMemberAsync(int roomId, int userId);
}