namespace ReservationService.Domain.Entities;

public class Table
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public string TableNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<ReservationService.Domain.AggregateRoots.Reservation> Reservations { get; set; } = new List<ReservationService.Domain.AggregateRoots.Reservation>();
}

