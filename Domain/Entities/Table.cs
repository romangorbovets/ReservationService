namespace ReservationService.Domain.Entities;

public class Table : Entity
{
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;
    public string TableNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<ReservationService.Domain.AggregateRoots.Reservation> Reservations { get; set; } = new List<ReservationService.Domain.AggregateRoots.Reservation>();
}