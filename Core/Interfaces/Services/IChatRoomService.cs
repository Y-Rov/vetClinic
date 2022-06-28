using Core.Entities;

namespace Core.Interfaces.Services;

public interface IChatRoomService
{
    Task Create(ChatRoom chatRoom);
    Task<IEnumerable<User>> GetParticipants();
    Task AddMember(User user);
    Task KickMember(int userId);
    Task Deactivate();
}