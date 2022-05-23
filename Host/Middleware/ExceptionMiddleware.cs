using Core.Entities;
using Core.Models;
using DataAccess.Context;
using WebApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Host.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        private readonly DateTime _dateTime = DateTime.Now;

        public ExceptionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext!.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.Unauthorized,
                    $"{ex.Message}. Path:{context.Request.Path}.", ex.ValidationFailure);
            }
            catch (NotFoundException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext!.SaveChangesAsync();
                }
                await HandleExeptionAsync(context, HttpStatusCode.Unauthorized,
                                   $"{ex.Message}. Path:{context.Request.Path}.", ex.ValidationFailure);
            }
            catch (ForbidException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext!.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.Unauthorized,
                     $"{ex.Message}. Path:{context.Request.Path}.", ex.ValidationFailure);
            }
            catch (BadRequestException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext!.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.Unauthorized,
                    $"{ex.Message}. Path:{context.Request.Path}.", ex.ValidationFailure);
            }
            catch (DivideByZeroException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext!.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (HttpRequestException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (DbUpdateConcurrencyException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (DbUpdateException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (RouteCreationException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (KeyNotFoundException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (WebException ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (Exception ex)
            {
                using (var serviceScope = _serviceProvider.CreateScope())
                {
                    var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                    dataContext!.Add(new ExceptionEntity(ex.GetType().Name, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                    await dataContext.SaveChangesAsync();
                }

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");

            }
        }

        private Task HandleExeptionAsync(HttpContext context, HttpStatusCode errorCode, string errorMessage)
        {
            context.Response.ContentType = "appliaction/json";
            context.Response.StatusCode = (int)errorCode;
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = errorMessage
            }.ToString());
        }
        private Task HandleExeptionAsync(HttpContext context, HttpStatusCode errorCode, string errorMessage, List<ValidationFailure> validationFailures)
        {
            context.Response.ContentType = "appliaction/json";
            context.Response.StatusCode = (int)errorCode;
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = errorMessage
            }.ToString() + validationFailures.ToString());
        }
    }
}
