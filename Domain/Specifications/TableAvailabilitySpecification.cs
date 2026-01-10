using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Specifications;

/// <summary>
/// Спецификация для проверки доступности столика в указанный временной диапазон
/// Гарантирует, что один и тот же столик не будет забронирован двумя клиентами на одно и то же время
/// </summary>
public class TableAvailabilitySpecification
{
    /// <summary>
    /// Проверить доступность столика через репозиторий
    /// </summary>
    public static async Task<(bool IsAvailable, string? Reason)> CheckAvailabilityAsync(
        ITableRepository tableRepository,
        IReservationRepository reservationRepository,
        Guid tableId,
        TimeRange timeRange,
        int numberOfGuests,
        Guid? excludeReservationId = null,
        CancellationToken cancellationToken = default)
    {
        var table = await tableRepository.GetByIdWithDetailsAsync(tableId, cancellationToken);
        
        if (table == null)
            return (false, "Table not found");

        if (!table.IsActive)
            return (false, "Table is not active");

        if (!table.CanAccommodate(numberOfGuests))
            return (false, $"Table cannot accommodate {numberOfGuests} guests. Maximum capacity: {table.Capacity}");

        // Проверяем конфликтующие резервации через репозиторий
        var conflictingReservations = await reservationRepository.GetActiveReservationsForTableAsync(
            tableId,
            timeRange,
            excludeReservationId,
            cancellationToken);

        if (conflictingReservations.Any())
            return (false, "Table is already reserved for this time period");

        return (true, null);
    }

    /// <summary>
    /// Проверить доступность столика (синхронная версия для работы с уже загруженной сущностью)
    /// </summary>
    public static bool IsSatisfiedBy(
        Table table,
        TimeRange timeRange,
        Guid? excludeReservationId = null)
    {
        if (!table.IsActive)
            return false;

        if (!table.IsAvailableFor(timeRange))
            return false;

        var conflictingReservations = table.Reservations
            .Where(r => r.IsActive())
            .Where(r => excludeReservationId == null || r.Id != excludeReservationId.Value)
            .Where(r => r.TimeRange.OverlapsWith(timeRange))
            .ToList();

        return !conflictingReservations.Any();
    }

    /// <summary>
    /// Проверить вместимость столика
    /// </summary>
    public static bool CanAccommodateGuests(Table table, int numberOfGuests)
    {
        return table.CanAccommodate(numberOfGuests);
    }

    /// <summary>
    /// Проверить доступность столика (синхронная версия)
    /// </summary>
    public static (bool IsAvailable, string? Reason) CheckAvailability(
        Table table,
        TimeRange timeRange,
        int numberOfGuests,
        Guid? excludeReservationId = null)
    {
        if (!table.IsActive)
            return (false, "Table is not active");

        if (!table.CanAccommodate(numberOfGuests))
            return (false, $"Table cannot accommodate {numberOfGuests} guests. Maximum capacity: {table.Capacity}");

        if (!IsSatisfiedBy(table, timeRange, excludeReservationId))
            return (false, "Table is already reserved for this time period");

        return (true, null);
    }
}
