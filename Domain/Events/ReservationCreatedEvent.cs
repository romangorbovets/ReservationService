using ReservationService.Domain.Common;

namespace ReservationService.Domain.Events;

/// <summary>
/// Доменное событие: резервация создана
/// </summary>
public class ReservationCreatedEvent : DomainEvent
{
    public Guid ReservationId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TableId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public DateTime ReservationStartTime { get; private set; }
    public DateTime ReservationEndTime { get; private set; }
    public int NumberOfGuests { get; private set; }

    public ReservationCreatedEvent(
        Guid reservationId,
        Guid customerId,
        Guid tableId,
        Guid restaurantId,
        DateTime reservationStartTime,
        DateTime reservationEndTime,
        int numberOfGuests)
    {
        ReservationId = reservationId;
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
        ReservationStartTime = reservationStartTime;
        ReservationEndTime = reservationEndTime;
        NumberOfGuests = numberOfGuests;
    }
}





