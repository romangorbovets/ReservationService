using MediatR;
using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

/// <summary>
/// Обработчик команды подтверждения резервации
/// </summary>
public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.ReservationId, cancellationToken);

        if (reservation == null)
            throw new ArgumentException($"Reservation with id {request.ReservationId} not found", nameof(request.ReservationId));

        reservation.Confirm();
        _unitOfWork.Reservations.Update(reservation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}



