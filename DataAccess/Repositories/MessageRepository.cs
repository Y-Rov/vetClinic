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
        var message = await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return (message is not null);
    }
}