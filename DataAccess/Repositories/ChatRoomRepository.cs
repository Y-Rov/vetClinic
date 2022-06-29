using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ChatRoomRepository : Repository<ChatRoom>, IChatRoomRepository
{
    public ChatRoomRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }
    
    public async Task<bool> ExistsAsync(int id)
    {
        var chatRoom = await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return (chatRoom is not null);
    }
}