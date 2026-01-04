using ReservationService.Domain.Common;

namespace ReservationService.Domain.Events;

/// <summary>
/// Доменное событие: резервация отменена
/// </summary>
public class ReservationCancelledEvent : DomainEvent
{
    public Guid ReservationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TableId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public string? Reason { get; private set; }
    public bool IsAutoCancelled { get; private set; }

    public ReservationCancelledEvent(
        Guid reservationId,
        Guid customerId,
        Guid tableId,
        Guid restaurantId,
        string? reason = null,
        bool isAutoCancelled = false)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
        Reason = reason;
        IsAutoCancelled = isAutoCancelled;
    }
}





