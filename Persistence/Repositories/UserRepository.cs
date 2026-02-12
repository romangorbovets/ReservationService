using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Npgsql;
=======
>>>>>>> main
using ReservationService.Domain.Common.Exceptions;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.Specifications;

namespace ReservationService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetAsync(ISpecification<User> specification, CancellationToken cancellationToken = default)
    {
<<<<<<< HEAD
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        if (specification.Criteria is null)
        {
            throw new ArgumentException("Specification criteria cannot be null", nameof(specification));
        }

        try
        {
            return await _context.Users
                .Where(specification.Criteria)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
        {
            throw new InvalidOperationException($"Database error: {dbEx.Message}", dbEx);
        }
        catch (Npgsql.NpgsqlException npgsqlEx)
        {
            throw new InvalidOperationException($"PostgreSQL connection error: {npgsqlEx.Message}. Please check your database connection settings.", npgsqlEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error executing specification query: {ex.Message}", ex);
        }
=======
        return await _context.Users
            .Where(specification.Criteria)
            .FirstOrDefaultAsync(cancellationToken);
>>>>>>> main
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        return user;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? string.Empty;
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
    }
}