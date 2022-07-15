using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            !.Value;
    }
}