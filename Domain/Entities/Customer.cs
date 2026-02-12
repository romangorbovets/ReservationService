using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Entities;

public class Customer : Entity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Address? Address { get; private set; }
    public ContactInfo ContactInfo { get; private set; } = null!;

    private readonly List<Reservation> _reservations = new();
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    private Customer() { }

    public Customer(string firstName, string lastName, ContactInfo contactInfo, Address? address = null)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        Address = address;
    }

    public void UpdateContactInfo(ContactInfo contactInfo)
    {
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
    }

    public void UpdateAddress(Address? address)
    {
        Address = address;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
    }
}