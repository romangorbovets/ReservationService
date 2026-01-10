using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

/// <summary>
/// Команда для создания резервации
/// </summary>
public class CreateReservationCommand : ICommand<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid TableId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPriceAmount { get; set; }
    public string TotalPriceCurrency { get; set; } = "USD";
    public bool AutoCancellationEnabled { get; set; } = true;
    public TimeSpan? AutoCancellationTimeout { get; set; }
    public string? SpecialRequests { get; set; }
}



