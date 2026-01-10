using MediatR;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.Queries.GetReservation;

namespace ReservationService.Application.Features.Reservations.Queries.GetReservations;

/// <summary>
/// Обработчик запроса получения списка резерваций
/// </summary>
public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, List<ReservationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReservationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.AggregateRoots.Reservation> reservations;

        if (request.CustomerId.HasValue)
        {
            reservations = await _unitOfWork.Reservations.GetByCustomerIdAsync(request.CustomerId.Value, cancellationToken);
        }
        else if (request.TableId.HasValue)
        {
            reservations = await _unitOfWork.Reservations.GetByTableIdAsync(request.TableId.Value, cancellationToken);
        }
        else if (request.RestaurantId.HasValue)
        {
            reservations = await _unitOfWork.Reservations.GetByRestaurantIdAsync(request.RestaurantId.Value, cancellationToken);
        }
        else
        {
            reservations = await _unitOfWork.Reservations.GetAllAsync(cancellationToken);
        }

        return reservations
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                TableId = r.TableId,
                RestaurantId = r.RestaurantId,
                StartTime = r.TimeRange.StartTime,
                EndTime = r.TimeRange.EndTime,
                NumberOfGuests = r.NumberOfGuests,
                Status = r.Status.Value,
                TotalPriceAmount = r.TotalPrice.Amount,
                TotalPriceCurrency = r.TotalPrice.Currency,
                SpecialRequests = r.SpecialRequests,
                CreatedAt = r.CreatedAt
            })
            .ToList();
    }
}

