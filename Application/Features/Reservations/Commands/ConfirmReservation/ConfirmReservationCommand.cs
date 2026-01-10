using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

/// <summary>
/// Команда для подтверждения резервации
/// </summary>
public class ConfirmReservationCommand : ICommand
{
    public Guid ReservationId { get; set; }
}



