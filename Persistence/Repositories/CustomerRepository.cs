using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Persistence;

namespace ReservationService.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для работы с клиентами
/// </summary>
public class CustomerRepository : BaseRepository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await DbSet
            .FirstOrDefaultAsync(c => c.ContactInfo.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await DbSet
            .AnyAsync(c => c.ContactInfo.Email.ToLower() == email.ToLower(), cancellationToken);
    }
}

