using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public ContactInfo ContactInfo { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<ReservationService.Domain.AggregateRoots.Reservation> Reservations { get; set; } = new List<ReservationService.Domain.AggregateRoots.Reservation>();
}

