using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.DTOs;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.Specifications;

namespace ReservationService.Application.Features.Reservations.Commands.GetReservation;

public class GetReservationCommandHandler : ICommandHandler<GetReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationCommandHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ReservationDto> Handle(GetReservationCommand command, CancellationToken cancellationToken = default)
    {
        var specification = new ReservationSpecification(command.ReservationId);
        var reservation = await _reservationRepository.GetAsync(specification, cancellationToken);

        if (reservation is null)
        {
            throw new KeyNotFoundException($"Reservation with id '{command.ReservationId}' not found.");
        }

        return new ReservationDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            TableId = reservation.TableId,
            RestaurantId = reservation.RestaurantId,
            StartTime = reservation.TimeRange.StartTime,
            EndTime = reservation.TimeRange.EndTime,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status,
            TotalPrice = reservation.TotalPrice.Amount,
            Currency = reservation.TotalPrice.Currency,
            AutoCancellationEnabled = reservation.AutoCancellationSettings?.IsEnabled ?? false,
            CancellationTimeout = reservation.AutoCancellationSettings?.CancellationTimeout,
            SpecialRequests = reservation.SpecialRequests,
            Notes = reservation.Notes,
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt,
            ConfirmedAt = reservation.ConfirmedAt,
            CancelledAt = reservation.CancelledAt,
            CancellationReason = reservation.CancellationReason,
            CompletedAt = reservation.CompletedAt
        };
    }
}