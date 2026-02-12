using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.GetAvailability;

public record GetAvailabilityCommand(
    Guid RestaurantId,
    DateTime StartTime,
    DateTime EndTime,
    int NumberOfGuests) : ICommand<List<Guid>>;