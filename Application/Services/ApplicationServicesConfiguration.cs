using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using WebApi.Validators;

namespace Application.Services
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ProcedureValidator>();
            });

        }
    }
}
