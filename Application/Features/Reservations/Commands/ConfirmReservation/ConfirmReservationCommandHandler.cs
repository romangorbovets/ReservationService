using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationService.Persistence;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

/// <summary>
/// Обработчик команды подтверждения резервации
/// </summary>
public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand>
{
    private readonly ApplicationDbContext _context;

    public ConfirmReservationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null)
            throw new ArgumentException($"Reservation with id {request.ReservationId} not found", nameof(request.ReservationId));

        reservation.Confirm();
        await _context.SaveChangesAsync(cancellationToken);
    }
}


