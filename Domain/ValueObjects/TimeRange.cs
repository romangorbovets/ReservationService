namespace ReservationService.Domain.ValueObjects;

/// <summary>
/// Value Object для представления временного диапазона резервации
/// </summary>
public class TimeRange
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan Duration => EndTime - StartTime;

    private TimeRange() { } // Для EF Core

    public TimeRange(DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time", nameof(startTime));

        if (startTime < DateTime.UtcNow.AddMinutes(-1))
            throw new ArgumentException("Start time cannot be in the past", nameof(startTime));

        StartTime = startTime;
        EndTime = endTime;
    }

    public bool OverlapsWith(TimeRange other)
    {
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    public bool Contains(DateTime dateTime)
    {
        return dateTime >= StartTime && dateTime <= EndTime;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TimeRange other)
            return false;

        return StartTime == other.StartTime && EndTime == other.EndTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }

    public static bool operator ==(TimeRange? left, TimeRange? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TimeRange? left, TimeRange? right)
    {
        return !Equals(left, right);
    }
}







