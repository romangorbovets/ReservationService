using ReservationService.Domain.Entities;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Specifications;

/// <summary>
/// Спецификация для проверки доступности столика в указанный временной диапазон
/// Гарантирует, что один и тот же столик не будет забронирован двумя клиентами на одно и то же время
/// </summary>
public class TableAvailabilitySpecification
{
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

    public static bool CanAccommodateGuests(Table table, int numberOfGuests)
    {
        return table.CanAccommodate(numberOfGuests);
    }

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






