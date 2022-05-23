using Application.Configuration;
using Core.DTO;
using Core.Entities;
using DataAccess.Context;
using DataAccess.Repositories;
using Host.Configurations;
using Host.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/Nlog.config"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSystemServices();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = true
        };
    });

builder.Services.AddDbContext<ClinicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ClinicContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
