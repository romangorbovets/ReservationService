namespace ReservationService.Domain.ValueObjects;

/// <summary>
/// Value Object для представления статуса резервации
/// </summary>
public class ReservationStatus
{
    public string Value { get; private set; }

    public static ReservationStatus Pending => new("Pending");
    public static ReservationStatus Confirmed => new("Confirmed");
    public static ReservationStatus Cancelled => new("Cancelled");
    public static ReservationStatus AutoCancelled => new("AutoCancelled");
    public static ReservationStatus Completed => new("Completed");
    public static ReservationStatus NoShow => new("NoShow");

    private ReservationStatus() { } // Для EF Core

    private ReservationStatus(string value)
    {
        Value = value;
    }

    public static ReservationStatus FromString(string value)
    {
        return value?.ToLowerInvariant() switch
        {
            "pending" => Pending,
            "confirmed" => Confirmed,
            "cancelled" => Cancelled,
            "autocancelled" => AutoCancelled,
            "completed" => Completed,
            "noshow" => NoShow,
            _ => throw new ArgumentException($"Invalid reservation status: {value}", nameof(value))
        };
    }

    public bool CanTransitionTo(ReservationStatus newStatus)
    {
        return Value switch
        {
            "Pending" => newStatus.Value is "Confirmed" or "Cancelled" or "AutoCancelled",
            "Confirmed" => newStatus.Value is "Completed" or "Cancelled" or "AutoCancelled" or "NoShow",
            "Cancelled" => false,
            "AutoCancelled" => false,
            "Completed" => false,
            "NoShow" => false,
            _ => false
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ReservationStatus other)
            return false;

        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator ==(ReservationStatus? left, ReservationStatus? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ReservationStatus? left, ReservationStatus? right)
    {
        return !Equals(left, right);
    }
}







