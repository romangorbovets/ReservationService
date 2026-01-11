using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Address Address { get; set; } = null!;
    public ContactInfo ContactInfo { get; set; } = null!;
    public string? TimeZone { get; set; }
    public TimeSpan? OpeningTime { get; set; }
    public TimeSpan? ClosingTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<ReservationService.Domain.AggregateRoots.Reservation> Reservations { get; set; } = new List<ReservationService.Domain.AggregateRoots.Reservation>();
    public ICollection<Table> Tables { get; set; } = new List<Table>();
}

