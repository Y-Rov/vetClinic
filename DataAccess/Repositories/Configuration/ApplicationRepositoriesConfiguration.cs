using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Repositories.Configuration
{
    public static class ApplicationRepositoriesConfiguration
    {
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IProcedureRepository, ProcedureRepository>();
            services.AddScoped<IExceptionEntityRepository, ExceptionEntityRepository>();
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
        }
    }
}
