using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;

namespace ReservationService.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для работы с резервациями
/// </summary>
public class ReservationRepository : BaseRepository<Reservation, Guid>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
                .ThenInclude(t => t.Restaurant)
            .Include(r => r.Restaurant)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Table)
            .Include(r => r.Restaurant)
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.TimeRange.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Customer)
            .Where(r => r.TableId == tableId)
            .OrderByDescending(r => r.TimeRange.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .Where(r => r.RestaurantId == restaurantId)
            .OrderByDescending(r => r.TimeRange.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsForTableAsync(
        Guid tableId,
        TimeRange timeRange,
        Guid? excludeReservationId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(r => r.TableId == tableId)
            .Where(r => r.Status.Value == "Pending" || r.Status.Value == "Confirmed")
            .Where(r => r.TimeRange.StartTime < timeRange.EndTime && r.TimeRange.EndTime > timeRange.StartTime);

        if (excludeReservationId.HasValue)
        {
            query = query.Where(r => r.Id != excludeReservationId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsRequiringAutoCancellationAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(r => r.Status.Value == "Pending")
            .Where(r => r.AutoCancellationSettings.IsEnabled)
            .ToListAsync(cancellationToken);
    }
}

