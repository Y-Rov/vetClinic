using System.Reflection;
using System.Linq;
using Core.Entities;
using DataAccess;
using IdentityServer.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.ServiceExtensions;

public static class ServicesInstaller
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        // services.AddCors(opts =>
        //      opts.AddPolicy("AllowAll",
        //          p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        
        services.AddDbContext<ApplicationContext>(opts => 
            opts.UseSqlServer(config.GetConnectionString("Default")));
        
        services.AddIdentity<User, IdentityRole<int>>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();

        var assembly = typeof(Program).Assembly.GetName().Name;
        
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddOperationalStore(opts =>
            {
                opts.ConfigureDbContext = builder => builder.UseSqlServer(config.GetConnectionString("Default"), 
                    opt => opt.MigrationsAssembly(assembly));
            })
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApiResources())
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryClients(Config.GetClients())
            .AddAspNetIdentity<User>();

        services.AddScoped<IProfileService, ProfileService>();
    }
}