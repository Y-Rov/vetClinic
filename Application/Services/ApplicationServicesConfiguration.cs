using Application.Services;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NLog;

public static class ApplicationServicesConfiguration
{
    public static void AddApplicationService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager,LoggerManager>();
    }
}