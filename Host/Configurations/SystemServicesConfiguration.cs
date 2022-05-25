﻿using Application.Services;
using Core.Interfaces;
using Core.ViewModels.User;
using FluentValidation.AspNetCore;
using WebApi.Validators;

namespace Host.Configurations
{
    public static class SystemServicesConfiguration
    {
        public static void AddSystemServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddScoped<UserValidator<UserUpdateViewModel>>();

            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ProcedureViewModelBaseValidator>();
            });
        }
    }
}
