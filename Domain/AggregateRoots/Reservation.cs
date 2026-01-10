using ReservationService.Domain.Entities;
using ReservationService.Domain.ValueObjects;
using ReservationService.Domain.Events;
using ReservationService.Domain.Common;

namespace ReservationService.Domain.AggregateRoots;

/// <summary>
/// Aggregate Root для резервации столика в ресторане
/// </summary>
public class Reservation
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid TableId { get; private set; }
    public Guid RestaurantId { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public int NumberOfGuests { get; private set; }
    public ReservationStatus Status { get; private set; }
    public Money TotalPrice { get; private set; }
    public AutoCancellationSettings AutoCancellationSettings { get; private set; }
    public string? SpecialRequests { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Customer Customer { get; private set; } = null!;
    public Table Table { get; private set; } = null!;
    public Restaurant Restaurant { get; private set; } = null!;

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Reservation() { } // Для EF Core

    public Reservation(
        Guid customerId,
        Guid tableId,
        Guid restaurantId,
        TimeRange timeRange,
        int numberOfGuests,
        Money totalPrice,
        AutoCancellationSettings? autoCancellationSettings = null,
        string? specialRequests = null)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        TableId = tableId;
        RestaurantId = restaurantId;
        TimeRange = timeRange ?? throw new ArgumentNullException(nameof(timeRange));
        
        if (numberOfGuests <= 0)
            throw new ArgumentException("Number of guests must be greater than zero", nameof(numberOfGuests));
        
        NumberOfGuests = numberOfGuests;
        Status = ReservationStatus.Pending;
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
        AutoCancellationSettings = autoCancellationSettings ?? new AutoCancellationSettings(isEnabled: true);
        SpecialRequests = specialRequests;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationCreatedEvent(
            Id,
            CustomerId,
            TableId,
            RestaurantId,
            TimeRange.StartTime,
            TimeRange.EndTime,
            NumberOfGuests));
    }

    public void Confirm()
    {
        if (!Status.CanTransitionTo(ReservationStatus.Confirmed))
            throw new InvalidOperationException($"Cannot confirm reservation with status: {Status}");

        Status = ReservationStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationConfirmedEvent(
            Id,
            CustomerId,
            TableId,
            RestaurantId,
            TimeRange.StartTime));
    }

    public void Cancel(string? reason = null, bool isAutoCancelled = false)
    {
        var targetStatus = isAutoCancelled ? ReservationStatus.AutoCancelled : ReservationStatus.Cancelled;
        
        if (!Status.CanTransitionTo(targetStatus))
            throw new InvalidOperationException($"Cannot cancel reservation with status: {Status}");

        Status = targetStatus;
        CancelledAt = DateTime.UtcNow;
        CancellationReason = reason ?? (isAutoCancelled ? "Automatically cancelled due to timeout" : null);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationCancelledEvent(
            Id,
            CustomerId,
            TableId,
            RestaurantId,
            CancellationReason,
            isAutoCancelled));
    }

    public void MarkAsCompleted()
    {
        if (!Status.CanTransitionTo(ReservationStatus.Completed))
            throw new InvalidOperationException($"Cannot mark as completed reservation with status: {Status}");

        Status = ReservationStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationCompletedEvent(
            Id,
            CustomerId,
            TableId,
            RestaurantId));
    }

    public void MarkAsNoShow()
    {
        if (!Status.CanTransitionTo(ReservationStatus.NoShow))
            throw new InvalidOperationException($"Cannot mark as no-show reservation with status: {Status}");

        Status = ReservationStatus.NoShow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTimeRange(TimeRange newTimeRange)
    {
        if (Status.Value != "Pending" && Status.Value != "Confirmed")
            throw new InvalidOperationException($"Cannot update time range for reservation with status: {Status}");

        TimeRange = newTimeRange ?? throw new ArgumentNullException(nameof(newTimeRange));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNumberOfGuests(int numberOfGuests)
    {
        if (numberOfGuests <= 0)
            throw new ArgumentException("Number of guests must be greater than zero", nameof(numberOfGuests));

        if (Status.Value != "Pending" && Status.Value != "Confirmed")
            throw new InvalidOperationException($"Cannot update number of guests for reservation with status: {Status}");

        NumberOfGuests = numberOfGuests;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSpecialRequests(string? specialRequests)
    {
        SpecialRequests = specialRequests;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTotalPrice(Money newPrice)
    {
        if (Status.Value != "Pending")
            throw new InvalidOperationException($"Cannot update price for reservation with status: {Status}");

        TotalPrice = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
        UpdatedAt = DateTime.UtcNow;
    }

    public bool ShouldAutoCancel(DateTime currentTime)
    {
        if (Status.Value != "Pending")
            return false;

        return AutoCancellationSettings.ShouldAutoCancel(TimeRange.StartTime, currentTime);
    }

    public void TryAutoCancel(DateTime currentTime)
    {
        if (ShouldAutoCancel(currentTime))
        {
            Cancel(isAutoCancelled: true);
        }
    }

    public bool IsOverlappingWith(Reservation other)
    {
        if (Id == other.Id || TableId != other.TableId)
            return false;

        return TimeRange.OverlapsWith(other.TimeRange);
    }

    public bool IsActive()
    {
        return Status.Value is "Pending" or "Confirmed";
    }

    private void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}






