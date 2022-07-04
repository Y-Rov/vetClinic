using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    public MessageRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var message = await DbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return (message is not null);
    }

    public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
    {
        var userChatRooms = _clinicContext.UserChatRooms
            .Include(ur => ur.LastReadMessage)
            .Where(ur => ur.UserId == userId);
        
        var messagesSet = DbSet.Include(m => m.ChatRoom);

        return await userChatRooms.GroupJoin(messagesSet,
                ur => ur.ChatRoomId,
                m => m.ChatRoomId,
                (userChatRoom, messages) => messages.Where(m => m.SentAt > userChatRoom.LastReadMessage!.SentAt))
            .SelectMany(x => x)
            .AsNoTracking()
            .ToListAsync();
    }
}