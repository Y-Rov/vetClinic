using Application.Configuration;
using Core.Entities;
using DataAccess.Context;
using DataAccess.Repositories;
using Host.Configurations;
using Host.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/Nlog.config"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSystemServices();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClinicContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ClinicContext>();


//Start ConfigureAutoMapper
var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddMaps(typeof(WebApi.AutoMapper.UserProfile));
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
//End ConfigureAutoMapper

var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddApplicationMiddleware();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
