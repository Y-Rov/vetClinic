using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using WebApi.SignalR;

namespace WebApi.SignalR.Configurations;

public static class SignalRConfigurations
{
    public static void AddAuthenticationForSignalRHubs(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs/messages")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    }

    public static void AddUserIdProviderForSignalR(this IServiceCollection services)
    {
        services.AddSingleton<IUserIdProvider, UserIdProvider>();
    }
}