using IdentityServer.ServiceExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

var config = builder.Configuration;
builder.Services.AddServices(config);

var app = builder.Build();

app.UseIdentityServer();
app.Run();