using ReservationService.Domain.Enums;

namespace ReservationService.Application.Features.Reservations.DTOs;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid TableId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfGuests { get; set; }
    public ReservationStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public bool AutoCancellationEnabled { get; set; }
    public TimeSpan? CancellationTimeout { get; set; }
    public string? SpecialRequests { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CompletedAt { get; set; }
}