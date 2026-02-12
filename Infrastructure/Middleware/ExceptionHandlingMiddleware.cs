using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
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
            var statusCode = GetStatusCode(ex);
            var logLevel = statusCode == System.Net.HttpStatusCode.NotFound 
                ? LogLevel.Information 
                : LogLevel.Error;
            
            _logger.Log(logLevel, ex, "Произошла необработанная ошибка: {Type}, {Message}, StatusCode: {StatusCode}", 
                ex.GetType().FullName, ex.Message, (int)statusCode);
            
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Ответ уже начат, невозможно обработать исключение");
                return;
            }

            try
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception handleEx)
            {
                _logger.LogError(handleEx, "Ошибка при обработке исключения: {Type}, {Message}, StackTrace: {StackTrace}", 
                    handleEx.GetType().FullName, handleEx.Message, handleEx.StackTrace);
            }
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Ответ уже начат, невозможно обработать исключение");
            return;
        }

        
        _logger.LogWarning("HandleExceptionAsync вызван. Тип исключения: {ExceptionType}, FullName: {FullName}, Message: {Message}, InnerException: {InnerException}", 
            exception.GetType().Name, 
            exception.GetType().FullName, 
            exception.Message,
            exception.InnerException?.GetType().FullName ?? "null");

        var httpStatusCode = GetStatusCode(exception);
        var statusCode = (int)httpStatusCode;
        
        _logger.LogWarning("GetStatusCode вернул: {HttpStatusCode} ({StatusCode}). Тип исключения: {ExceptionType}", 
            httpStatusCode, statusCode, exception.GetType().Name);
        
        object response;

        if (exception is ValidationException validationException)
        {
            _logger.LogInformation("Обработка ValidationException с {ErrorCount} ошибками валидации", 
                validationException.Errors.Count);
            
            response = new
            {
                error = new
                {
                    message = exception.Message,
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow,
                    errors = validationException.Errors
                }
            };
        }
        else if (exception is DbUpdateException dbUpdateException)
        {
            var innerException = dbUpdateException.InnerException;
            var innerMessage = innerException?.Message ?? dbUpdateException.Message;
            var innerType = innerException?.GetType().Name ?? "null";
            
            _logger.LogError(dbUpdateException, 
                "Обработка DbUpdateException. Внутреннее исключение: {InnerExceptionType} - {InnerMessage}", 
                innerType,
                innerMessage);
            
            
            response = new
            {
                error = new
                {
                    message = $"Database error: {innerMessage}",
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow,
                    innerException = innerException != null ? new
                    {
                        type = innerType,
                        message = innerMessage,
                        sqlState = (innerException as Npgsql.PostgresException)?.SqlState,
                        constraintName = (innerException as Npgsql.PostgresException)?.ConstraintName,
                        tableName = (innerException as Npgsql.PostgresException)?.TableName
                    } : null
                }
            };
        }
        else if (exception is KeyNotFoundException keyNotFoundException)
        {
            _logger.LogWarning("Обработка KeyNotFoundException: {Message}, StatusCode будет: {StatusCode}", 
                keyNotFoundException.Message, statusCode);
            
            
            if (statusCode != 404)
            {
                _logger.LogError("ОШИБКА: StatusCode для KeyNotFoundException должен быть 404, но получен {StatusCode}. Принудительно устанавливаю 404.", statusCode);
                statusCode = 404;
            }
            
            response = new
            {
                error = new
                {
                    message = exception.Message,
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow
                }
            };
        }
        else if (exception.InnerException is KeyNotFoundException innerKeyNotFoundException)
        {
            _logger.LogWarning("Обработка исключения с KeyNotFoundException в InnerException: {Message}, StatusCode будет: 404", 
                innerKeyNotFoundException.Message);
            
            statusCode = 404;
            
            response = new
            {
                error = new
                {
                    message = innerKeyNotFoundException.Message,
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow
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
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow
                }
            };
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        try
        {
            
            context.Response.Clear();
            
            
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            
            
            await context.Response.WriteAsync(jsonResponse);
            
            _logger.LogInformation("Ответ об ошибке успешно записан. StatusCode: {StatusCode}, Message: {Message}, BodyLength: {Length}", 
                statusCode, exception.Message, jsonResponse.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось записать ответ об ошибке: {Message}. HasStarted: {HasStarted}, StatusCode: {StatusCode}, ExceptionType: {ExceptionType}", 
                ex.Message, context.Response.HasStarted, context.Response.StatusCode, ex.GetType().Name);
            
            
            if (!context.Response.HasStarted)
            {
                try
                {
                    context.Response.StatusCode = statusCode;
                }
                catch
                {
                    
                }
            }
        }
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        
        if (exception is KeyNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }
        
        
        if (exception.InnerException is KeyNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }
        
        return exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            ArgumentNullException or ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            FileNotFoundException => HttpStatusCode.NotFound,
            DuplicateEntityException => HttpStatusCode.Conflict,
            InvalidOperationException => HttpStatusCode.InternalServerError,
            NotSupportedException => HttpStatusCode.MethodNotAllowed,
            _ => HttpStatusCode.InternalServerError
        };
    }
}