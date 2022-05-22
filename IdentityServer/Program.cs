using IdentityServer.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddServices(config);

var app = builder.Build();

app.UseIdentityServer();
app.Run();