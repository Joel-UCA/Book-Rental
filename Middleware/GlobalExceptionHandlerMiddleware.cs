using Book_Rental.DTOs.Responses;
using Book_Rental.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Book_Rental.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An error occurred while processing your request.";

            // Determine status code and message based on exception type
            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    _logger.LogWarning(notFoundException, "Not Found: {Message}", message);
                    break;

                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequestException.Message;
                    _logger.LogWarning(badRequestException, "Bad Request: {Message}", message);
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorizedException.Message;
                    _logger.LogWarning(unauthorizedException, "Unauthorized: {Message}", message);
                    break;

                case ForbiddenException forbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    message = forbiddenException.Message;
                    _logger.LogWarning(forbiddenException, "Forbidden: {Message}", message);
                    break;

                case ConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    message = conflictException.Message;
                    _logger.LogWarning(conflictException, "Conflict: {Message}", message);
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message,
                Details = _env.IsDevelopment() ? exception.StackTrace : null
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }
}
