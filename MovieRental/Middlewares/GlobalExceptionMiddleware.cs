using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieRental.Exceptions;
using System.Net;
using System.Text.Json;

namespace MovieRental.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // ✅ CONSTRUTOR CORRETO - RequestDelegate vem do ASP.NET Core
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails();

            switch (exception)
            {
                case CustomerNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Customer not found";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Customer not found");
                    break;

                case MovieNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Movie not found";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Movie not found");
                    break;

                case PaymentFailedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Payment failed";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Payment failed");
                    break;

                case ArgumentException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Invalid request";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Invalid argument");
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Title = "Internal server error";
                    problemDetails.Detail = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred";
                    _logger.LogError(exception, "Unhandled exception");
                    break;
            }

            problemDetails.Status = context.Response.StatusCode;
            problemDetails.Instance = context.Request.Path;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(problemDetails, options);
            await context.Response.WriteAsync(json);
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}