using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;

namespace ReservationService.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для работы со столиками
/// </summary>
public class TableRepository : BaseRepository<Table, Guid>, ITableRepository
{
    public TableRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Table?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.Restaurant)
            .Include(t => t.Reservations)
                .ThenInclude(r => r.Customer)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.Reservations.Where(r => r.Status.Value == "Pending" || r.Status.Value == "Confirmed"))
            .Where(t => t.RestaurantId == restaurantId)
            .OrderBy(t => t.TableNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetAvailableTablesAsync(
        Guid restaurantId,
        TimeRange timeRange,
        int numberOfGuests,
        CancellationToken cancellationToken = default)
    {
        var tables = await DbSet
            .Include(t => t.Reservations)
            .Where(t => t.RestaurantId == restaurantId)
            .Where(t => t.IsActive)
            .Where(t => t.Capacity >= numberOfGuests)
            .ToListAsync(cancellationToken);

        // Фильтруем столики, которые не имеют конфликтующих резерваций
        return tables.Where(table =>
        {
            var conflictingReservations = table.Reservations
                .Where(r => r.Status.Value == "Pending" || r.Status.Value == "Confirmed")
                .Where(r => r.TimeRange.StartTime < timeRange.EndTime && r.TimeRange.EndTime > timeRange.StartTime)
                .ToList();

            return !conflictingReservations.Any();
        }).ToList();
    }
}

