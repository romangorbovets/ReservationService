using ReservationService.Domain.Common;

namespace ReservationService.Domain.Events;

/// <summary>
/// Доменное событие: резервация подтверждена
/// </summary>
public class ReservationConfirmedEvent : DomainEvent
{
    public Guid ReservationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TableId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public DateTime ReservationStartTime { get; private set; }

    public ReservationConfirmedEvent(
        Guid reservationId,
        Guid customerId,
        Guid tableId,
        Guid restaurantId,
        DateTime reservationStartTime)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
        ReservationStartTime = reservationStartTime;
    }
}





