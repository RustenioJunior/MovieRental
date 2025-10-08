using Microsoft.AspNetCore.Mvc;
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
                // ✅ CUSTOMER EXCEPTIONS
                case CustomerNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Customer not found";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Customer not found");
                    break;

                case CustomerEmailAlreadyExistsException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    problemDetails.Title = "Customer already exists";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Customer email conflict");
                    break;

                // ✅ MOVIE EXCEPTIONS  
                case MovieNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Movie not found";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Movie not found");
                    break;

                // ✅ PAYMENT EXCEPTIONS
                case PaymentFailedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Payment failed";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Payment failed");
                    break;

                case InsufficientFundsException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.PaymentRequired;
                    problemDetails.Title = "Insufficient funds";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Insufficient funds");
                    break;

                // ✅ EXCEPTIONS GERAIS
                case ArgumentException ex:
                case InvalidOperationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Invalid request";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Invalid request");
                    break;

                case UnauthorizedAccessException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = ex.Message;
                    _logger.LogWarning(ex, "Unauthorized access");
                    break;

                // ✅ ERRO INTERNO (genérico)
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