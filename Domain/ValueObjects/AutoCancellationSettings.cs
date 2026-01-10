namespace ReservationService.Domain.ValueObjects;

/// <summary>
/// Value Object для настроек автоматической отмены резервации
/// </summary>
public class AutoCancellationSettings
{
    public bool IsEnabled { get; private set; }
    public TimeSpan? CancellationTimeout { get; private set; }

    private AutoCancellationSettings() { } // Для EF Core

    public AutoCancellationSettings(bool isEnabled, TimeSpan? cancellationTimeout = null)
    {
        IsEnabled = isEnabled;
        
        if (isEnabled && cancellationTimeout.HasValue && cancellationTimeout.Value <= TimeSpan.Zero)
            throw new ArgumentException("Cancellation timeout must be positive", nameof(cancellationTimeout));
        
        CancellationTimeout = cancellationTimeout ?? TimeSpan.FromMinutes(15);
    }

    public bool ShouldAutoCancel(DateTime reservationStartTime, DateTime currentTime)
    {
        if (!IsEnabled)
            return false;

        if (!CancellationTimeout.HasValue)
            return false;

        var timeUntilReservation = reservationStartTime - currentTime;
        return timeUntilReservation > TimeSpan.Zero && timeUntilReservation <= CancellationTimeout.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AutoCancellationSettings other)
            return false;

        return IsEnabled == other.IsEnabled && CancellationTimeout == other.CancellationTimeout;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsEnabled, CancellationTimeout);
    }

    public static bool operator ==(AutoCancellationSettings? left, AutoCancellationSettings? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AutoCancellationSettings? left, AutoCancellationSettings? right)
    {
        return !Equals(left, right);
    }
}






