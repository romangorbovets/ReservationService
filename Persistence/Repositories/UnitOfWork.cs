using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using ReservationService.Domain.Common.Exceptions;
using ReservationService.Domain.Repositories;

namespace ReservationService.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            _logger.LogWarning(ex, "Обнаружено нарушение уникального ограничения: {Message}", ex.InnerException?.Message ?? ex.Message);
            throw new DuplicateEntityException("Duplicate entity", ex);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            var constraintName = GetConstraintName(ex);
            var entityName = GetEntityNameFromConstraint(constraintName);
            
            _logger.LogWarning(ex, "Обнаружено нарушение внешнего ключа: {ConstraintName}, {Message}", 
                constraintName, ex.InnerException?.Message ?? ex.Message);
            
            throw new KeyNotFoundException(
                $"Related entity not found. Constraint: {constraintName}. " +
                $"Please ensure that the {entityName} exists before creating this reservation.", ex);
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            var innerMessage = innerException?.Message ?? ex.Message;
            var innerType = innerException?.GetType().Name ?? "null";
            
            _logger.LogError(ex, 
                "Ошибка при сохранении изменений в базе данных. " +
                "Тип исключения: {ExceptionType}, " +
                "Сообщение: {Message}, " +
                "Внутреннее исключение: {InnerExceptionType} - {InnerMessage}",
                ex.GetType().FullName,
                ex.Message,
                innerType,
                innerMessage);
            
            
            throw;
        }
    }

    private static bool IsForeignKeyViolation(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? string.Empty;
        return message.Contains("foreign key constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("violates foreign key constraint", StringComparison.OrdinalIgnoreCase) ||
               (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23503");
    }

    private static string GetConstraintName(DbUpdateException ex)
    {
        if (ex.InnerException is Npgsql.PostgresException pgEx)
        {
            return pgEx.ConstraintName ?? "unknown";
        }
        
        var message = ex.InnerException?.Message ?? string.Empty;
        var match = System.Text.RegularExpressions.Regex.Match(message, @"constraint ""([^""]+)""");
        return match.Success ? match.Groups[1].Value : "unknown";
    }

    private static string GetEntityNameFromConstraint(string constraintName)
    {
        if (constraintName.Contains("Customer", StringComparison.OrdinalIgnoreCase))
            return "Customer";
        if (constraintName.Contains("Table", StringComparison.OrdinalIgnoreCase))
            return "Table";
        if (constraintName.Contains("Restaurant", StringComparison.OrdinalIgnoreCase))
            return "Restaurant";
        
        return "related entity";
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? string.Empty;
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
    }
}