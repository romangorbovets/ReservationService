using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.CancelReservation;

public record CancelReservationCommand(
    Guid ReservationId,
    string? CancellationReason = null) : ICommand;