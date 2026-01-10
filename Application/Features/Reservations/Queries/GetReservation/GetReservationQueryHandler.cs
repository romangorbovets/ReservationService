using MediatR;
using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Queries.GetReservation;

/// <summary>
/// Обработчик запроса получения резервации
/// </summary>
public class GetReservationQueryHandler : IRequestHandler<GetReservationQuery, ReservationDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReservationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReservationDto> Handle(GetReservationQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.ReservationId, cancellationToken);

        if (reservation == null)
            throw new ArgumentException($"Reservation with id {request.ReservationId} not found", nameof(request.ReservationId));

        return new ReservationDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            TableId = reservation.TableId,
            RestaurantId = reservation.RestaurantId,
            StartTime = reservation.TimeRange.StartTime,
            EndTime = reservation.TimeRange.EndTime,
            NumberOfGuests = reservation.NumberOfGuests,
            Status = reservation.Status.Value,
            TotalPriceAmount = reservation.TotalPrice.Amount,
            TotalPriceCurrency = reservation.TotalPrice.Currency,
            SpecialRequests = reservation.SpecialRequests,
            CreatedAt = reservation.CreatedAt
        };
    }
}



