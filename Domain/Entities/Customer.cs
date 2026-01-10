namespace ReservationService.Domain.Entities;

/// <summary>
/// Сущность клиента
/// </summary>
public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public ValueObjects.ContactInfo ContactInfo { get; private set; }
    public ValueObjects.Address? Address { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<AggregateRoots.Reservation> _reservations = new();
    public IReadOnlyCollection<AggregateRoots.Reservation> Reservations => _reservations.AsReadOnly();

    private Customer() { } // Для EF Core

    public Customer(string firstName, string lastName, ValueObjects.ContactInfo contactInfo, ValueObjects.Address? address = null)
    {
        Id = Guid.NewGuid();
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        Address = address;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(ValueObjects.ContactInfo contactInfo)
    {
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(ValueObjects.Address? address)
    {
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        UpdatedAt = DateTime.UtcNow;
    }
}





