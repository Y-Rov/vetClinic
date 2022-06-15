using Application.Services;
using Core.Interfaces;
using Core.ViewModels.User;
using FluentValidation.AspNetCore;
using WebApi.Validators;
using WebApi.Validators.User;

namespace Host.Configurations
{
    public static class SystemServicesConfiguration
    {
        static public string AllowedOrigins = "frontend";

        public static void AddSystemServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ProcedureViewModelBaseValidator>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200");
                    policy.WithHeaders("*");
                    policy.WithMethods("*");
                });
            });
        }
    }
}
