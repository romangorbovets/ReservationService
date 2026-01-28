using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

public record ConfirmReservationCommand(Guid ReservationId) : ICommand;