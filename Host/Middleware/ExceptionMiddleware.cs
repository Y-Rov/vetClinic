﻿using Core.Entities;
using Core.Exceptions;
using Core.Models;
using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.Unauthorized,
                   $"{ex.Message}. Path:{context.Request.Path}.", ex.IdentityError!);
            }
            catch (NotFoundException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.NotFound,
                    $"{ex.Message}. Path:{context.Request.Path}.", ex.IdentityError!);
            }
            catch (ForbidException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.Forbidden,
                     $"{ex.Message}. Path:{context.Request.Path}.", ex.IdentityError!);
            }
            catch (BadRequestException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.BadRequest,
                    $"{ex.Message}. Path:{context.Request.Path}.", ex.IdentityError!);
            }
            catch (DivideByZeroException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (HttpRequestException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (DbUpdateConcurrencyException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (DbUpdateException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (RouteCreationException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");
            }

            catch (KeyNotFoundException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (WebException ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}.");
            }
            catch (Exception ex)
            {
                await AddExceptionAsync(ex, context, ex.GetType().Name);

                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                   $"{ex.Message}. Path:{context.Request.Path}.");

            }
        }

        private async Task AddExceptionAsync(Exception ex, HttpContext context, string exceptionType)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var dataContext = serviceScope.ServiceProvider.GetService<ClinicContext>();
                dataContext!.Add(new ExceptionEntity(exceptionType, _dateTime, ex.StackTrace ?? string.Empty, context.Request.Path));
                await dataContext!.SaveChangesAsync();
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

        private Task HandleExeptionAsync(HttpContext context, HttpStatusCode errorCode, string errorMessage, List<IdentityError> validationFailures)
        {
            context.Response.ContentType = "appliaction/json";
            context.Response.StatusCode = (int)errorCode;
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = errorMessage
            }.ToString() + validationFailures?.ToString());
        }
    }
}
