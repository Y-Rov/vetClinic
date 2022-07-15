using Application.Services;
using Core.Interfaces;
using FluentValidation.AspNetCore;
using WebApi.Validators;

namespace Host.Configurations
{
    public static class SystemServicesConfiguration
    {
        public const string AllowedOrigins = "frontend";

        public static void AddSystemServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
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
                    policy.WithExposedHeaders("X-Pagination");
                });
            });
        }
    }
}
