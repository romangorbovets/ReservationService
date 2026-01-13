using System.Net;
using System.Text.Json;
using ReservationService.Domain.Common.Exceptions;

namespace ReservationService.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
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
            _logger.LogError(ex, "Произошла необработанная ошибка: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)GetStatusCode(exception);

        object response;

        if (exception is ValidationException validationException)
        {
            response = new
            {
                error = new
                {
                    message = exception.Message,
                    statusCode = context.Response.StatusCode,
                    timestamp = DateTime.UtcNow,
                    errors = validationException.Errors
                }
            };
        }
        else
        {
            response = new
            {
                error = new
                {
                    message = exception.Message,
                    statusCode = context.Response.StatusCode,
                    timestamp = DateTime.UtcNow
                }
            };
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            ArgumentNullException or ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException or FileNotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException => HttpStatusCode.Conflict,
            NotSupportedException => HttpStatusCode.MethodNotAllowed,
            _ => HttpStatusCode.InternalServerError
        };
    }
}


