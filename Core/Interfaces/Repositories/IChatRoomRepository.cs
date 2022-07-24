using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IChatRoomRepository : IRepository<ChatRoom>
{
    Task<bool> ExistsAsync(int id);
}