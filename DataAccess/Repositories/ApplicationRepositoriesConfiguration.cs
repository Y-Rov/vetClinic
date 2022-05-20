using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Repositories
{
    public static class ApplicationRepositoriesConfiguration
    {
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
        }
    }
}
