namespace ReservationService.Domain.Entities;

/// <summary>
/// Сущность столика в ресторане
/// </summary>
public class Table
{
    public Guid Id { get; private set; }
    public Guid RestaurantId { get; private set; }
    public string TableNumber { get; private set; }
    public int Capacity { get; private set; }
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Restaurant Restaurant { get; private set; } = null!;

    private readonly List<AggregateRoots.Reservation> _reservations = new();
    public IReadOnlyCollection<AggregateRoots.Reservation> Reservations => _reservations.AsReadOnly();

    private Table() { } // Для EF Core

    public Table(
        Guid restaurantId,
        string tableNumber,
        int capacity,
        string? location = null)
    {
        Id = Guid.NewGuid();
        RestaurantId = restaurantId;
        TableNumber = tableNumber ?? throw new ArgumentNullException(nameof(tableNumber));
        
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero", nameof(capacity));
        
        Capacity = capacity;
        Location = location;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTableNumber(string tableNumber)
    {
        TableNumber = tableNumber ?? throw new ArgumentNullException(nameof(tableNumber));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateCapacity(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero", nameof(capacity));
        
        Capacity = capacity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLocation(string? location)
    {
        Location = location;
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

    public bool CanAccommodate(int numberOfGuests)
    {
        return IsActive && numberOfGuests > 0 && numberOfGuests <= Capacity;
    }

    public bool IsAvailableFor(ValueObjects.TimeRange timeRange)
    {
        if (!IsActive)
            return false;

        var conflictingReservations = _reservations
            .Where(r => r.IsActive() && r.TimeRange.OverlapsWith(timeRange))
            .ToList();

        return !conflictingReservations.Any();
    }
}





