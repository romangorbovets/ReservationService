using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Common.Exceptions;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;

namespace ReservationService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            throw new DuplicateEntityException("Duplicate entity", ex);
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? string.Empty;
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
    }
}