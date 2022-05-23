using Application.Services;
using Core.Interfaces;
using FluentValidation.AspNetCore;
using WebApi.Validators;

namespace Host.Configurations
{
    public static class SystemServicesConfiguration
    {
        public static void AddSystemServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ProcedureValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UserCreateDtoValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UserUpdateDtoValidator>();
            });
        }
    }
}
