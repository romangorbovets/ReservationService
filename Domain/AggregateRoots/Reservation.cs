namespace ReservationService.Domain.AggregateRoots;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Entities.Customer Customer { get; set; } = null!;
    public Guid TableId { get; set; }
    public Entities.Table Table { get; set; } = null!;
    public Guid RestaurantId { get; set; }
    public Entities.Restaurant Restaurant { get; set; } = null!;
    public ValueObjects.TimeRange TimeRange { get; set; } = null!;
    public int NumberOfGuests { get; set; }
    public string Status { get; set; } = string.Empty;
    public ValueObjects.Money TotalPrice { get; set; } = null!;
    public ValueObjects.AutoCancellationSettings AutoCancellationSettings { get; set; } = null!;
    public string? SpecialRequests { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CompletedAt { get; set; }
}



