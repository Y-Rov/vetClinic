using Core.Entities;
using Core.Interfaces.Services;

namespace Application.Services;

public class ChatRoomService : IChatRoomService
{
    public Task Create(ChatRoom chatRoom)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetParticipants()
    {
        throw new NotImplementedException();
    }

    public Task AddMember(User user)
    {
        throw new NotImplementedException();
    }

    public Task KickMember(int userId)
    {
        throw new NotImplementedException();
    }

    public Task Deactivate()
    {
        throw new NotImplementedException();
    }
}