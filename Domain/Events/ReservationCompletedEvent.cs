using ReservationService.Domain.Common;

namespace ReservationService.Domain.Events;

/// <summary>
/// Доменное событие: резервация завершена
/// </summary>
public class ReservationCompletedEvent : DomainEvent
{
    public Guid ReservationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TableId { get; private set; }
    public Guid RestaurantId { get; private set; }

    public ReservationCompletedEvent(
        Guid reservationId,
        Guid customerId,
        Guid tableId,
        Guid restaurantId)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
    }
}






