using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid CustomerId,
    Guid TableId,
    Guid RestaurantId,
    DateTime StartTime,
    DateTime EndTime,
    int NumberOfGuests,
    decimal TotalPriceAmount,
    string Currency = "USD",
    bool? AutoCancellationEnabled = null,
    TimeSpan? CancellationTimeout = null,
    string? SpecialRequests = null) : ICommand<Guid>;