using Microsoft.AspNetCore.SignalR;

namespace WebApi.SignalR.HubFilters;

public class IntUserIdentifierHubFilter : IHubFilter
{
    public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
    {
        var isIntIdentifier = Int32.TryParse(invocationContext.Context.UserIdentifier, out int userId);
        if (!isIntIdentifier)
            throw new HubException("User identifier have to be an integer");

        return await next(invocationContext);
    }
}