namespace ReservationService.Domain.Entities;

/// <summary>
/// Сущность ресторана в сети
/// </summary>
public class Restaurant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public ValueObjects.Address Address { get; private set; }
    public ValueObjects.ContactInfo ContactInfo { get; private set; }
    public string? TimeZone { get; private set; }
    public TimeSpan? OpeningTime { get; private set; }
    public TimeSpan? ClosingTime { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<Table> _tables = new();
    public IReadOnlyCollection<Table> Tables => _tables.AsReadOnly();

    private readonly List<AggregateRoots.Reservation> _reservations = new();
    public IReadOnlyCollection<AggregateRoots.Reservation> Reservations => _reservations.AsReadOnly();

    private Restaurant() { } // Для EF Core

    public Restaurant(
        string name,
        ValueObjects.Address address,
        ValueObjects.ContactInfo contactInfo,
        string? description = null,
        string? timeZone = null,
        TimeSpan? openingTime = null,
        TimeSpan? closingTime = null)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        Description = description;
        TimeZone = timeZone ?? "UTC";
        OpeningTime = openingTime;
        ClosingTime = closingTime;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(ValueObjects.Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(ValueObjects.ContactInfo contactInfo)
    {
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOperatingHours(TimeSpan? openingTime, TimeSpan? closingTime)
    {
        if (openingTime.HasValue && closingTime.HasValue && openingTime >= closingTime)
            throw new ArgumentException("Opening time must be before closing time");

        OpeningTime = openingTime;
        ClosingTime = closingTime;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsOpenAt(DateTime dateTime)
    {
        if (!IsActive)
            return false;

        if (!OpeningTime.HasValue || !ClosingTime.HasValue)
            return true;

        var localTime = dateTime;
        var timeOfDay = localTime.TimeOfDay;

        return timeOfDay >= OpeningTime.Value && timeOfDay <= ClosingTime.Value;
    }
}







