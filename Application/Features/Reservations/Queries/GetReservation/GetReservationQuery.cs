using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.Queries.GetReservation;

namespace ReservationService.Application.Features.Reservations.Queries.GetReservation;

/// <summary>
/// Запрос для получения резервации по ID
/// </summary>
public class GetReservationQuery : IQuery<ReservationDto>
{
    public Guid ReservationId { get; set; }
}

/// <summary>
/// DTO для резервации
/// </summary>
public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid TableId { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int NumberOfGuests { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPriceAmount { get; set; }
    public string TotalPriceCurrency { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}



