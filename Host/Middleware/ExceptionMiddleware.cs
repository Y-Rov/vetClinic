using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Host.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly DateTime _dateTime = DateTime.Now;
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DivideByZeroException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError,
                    $"{ex.Message}. Path:{context.Request.Path}. Query string:{context.Request.QueryString}");
            }
            catch (HttpRequestException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }

            catch (DbUpdateConcurrencyException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }

            catch (DbUpdateException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }

            catch (RouteCreationException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }

            catch (KeyNotFoundException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }
            catch (WebException ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");
            }
            catch (Exception ex)
            {
                await HandleExeptionAsync(context, HttpStatusCode.InternalServerError, "Error");

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
    }
}
