using Microsoft.EntityFrameworkCore;
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
        return await _context.Users
            .Where(specification.Criteria)
            .FirstOrDefaultAsync(cancellationToken);
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