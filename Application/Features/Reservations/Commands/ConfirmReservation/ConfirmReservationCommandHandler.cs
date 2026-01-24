using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Repositories;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

public class ConfirmReservationCommandHandler : ICommandHandler<ConfirmReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmReservationCommandHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmReservationCommand command, CancellationToken cancellationToken = default)
    {
        var reservation = await _reservationRepository.GetByIdAsync(command.ReservationId, cancellationToken);

        if (reservation is null)
        {
            throw new InvalidOperationException("Reservation not found");
        }

        await _reservationRepository.UpdateAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
