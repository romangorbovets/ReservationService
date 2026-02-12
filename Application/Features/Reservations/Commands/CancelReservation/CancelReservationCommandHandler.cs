using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.Specifications;

namespace ReservationService.Application.Features.Reservations.Commands.CancelReservation;

public class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelReservationCommandHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelReservationCommand command, CancellationToken cancellationToken = default)
    {
        var specification = new ReservationSpecification(command.ReservationId);
        var reservation = await _reservationRepository.GetAsync(specification, cancellationToken);

        if (reservation is null)
        {
            throw new KeyNotFoundException($"Reservation with id '{command.ReservationId}' not found.");
        }

        reservation.Cancel(command.CancellationReason);
        await _reservationRepository.UpdateAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}