using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
