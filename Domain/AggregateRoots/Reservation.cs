using ReservationService.Domain.Enums;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.AggregateRoots;

public class Reservation : AggregateRoot
{
    public Guid CustomerId { get; private set; }
    public Entities.Customer Customer { get; private set; } = null!;
    public Guid TableId { get; private set; }
    public Entities.Table Table { get; private set; } = null!;
    public Guid RestaurantId { get; private set; }
    public Entities.Restaurant Restaurant { get; private set; } = null!;
    public TimeRange TimeRange { get; private set; } = null!;
    public int NumberOfGuests { get; private set; }
    public ReservationStatus Status { get; private set; }
    public Money TotalPrice { get; private set; } = null!;
    public AutoCancellationSettings AutoCancellationSettings { get; private set; } = null!;
    public string? SpecialRequests { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Reservation() { }

    public Reservation(
        Guid customerId,
        Guid tableId,
        Guid restaurantId,
        TimeRange timeRange,
        int numberOfGuests,
        Money totalPrice,
        AutoCancellationSettings? autoCancellationSettings = null,
        string? specialRequests = null)
    {
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
        TimeRange = timeRange ?? throw new ArgumentNullException(nameof(timeRange));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfGuests);
        NumberOfGuests = numberOfGuests;
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
        AutoCancellationSettings = autoCancellationSettings ?? new AutoCancellationSettings();
        SpecialRequests = specialRequests;
        Status = ReservationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
}




