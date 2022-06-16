using IdentityServer.ServiceExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

var config = builder.Configuration;
builder.Services.AddServices(config);
builder.Services.AddCors(config =>
{
    config.AddDefaultPolicy(config => config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}
);

var app = builder.Build();
app.UseCors();
app.UseIdentityServer();
app.Run();