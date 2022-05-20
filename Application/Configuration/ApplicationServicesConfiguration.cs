using Application.Services;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAnimalService, AnimalService>();
        }
    }
}
