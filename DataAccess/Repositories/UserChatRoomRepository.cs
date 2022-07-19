using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class UserChatRoomRepository : Repository<UserChatRoom>, IUserChatRoomRepository
{
    public UserChatRoomRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }
}