using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.DTOs;

namespace ReservationService.Application.Features.Reservations.Commands.GetReservation;

public record GetReservationCommand(Guid ReservationId) : ICommand<ReservationDto>;