using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationService.Application.Features.Reservations.Queries.GetReservation;
using ReservationService.Persistence;

namespace ReservationService.Application.Features.Reservations.Queries.GetReservations;

/// <summary>
/// Обработчик запроса получения списка резерваций
/// </summary>
public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, List<ReservationDto>>
{
    private readonly ApplicationDbContext _context;

    public GetReservationsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reservations.AsQueryable();

        if (request.CustomerId.HasValue)
            query = query.Where(r => r.CustomerId == request.CustomerId.Value);

        if (request.TableId.HasValue)
            query = query.Where(r => r.TableId == request.TableId.Value);

        if (request.RestaurantId.HasValue)
            query = query.Where(r => r.RestaurantId == request.RestaurantId.Value);

        var reservations = await query
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
            .ToListAsync(cancellationToken);

        return reservations;
    }
}

