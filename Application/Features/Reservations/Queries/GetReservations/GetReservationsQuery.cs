using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.Queries.GetReservation;

namespace ReservationService.Application.Features.Reservations.Queries.GetReservations;

/// <summary>
/// Запрос для получения списка резерваций
/// </summary>
public class GetReservationsQuery : IQuery<List<ReservationDto>>
{
    public Guid? CustomerId { get; set; }
    public Guid? TableId { get; set; }
    public Guid? RestaurantId { get; set; }
}


