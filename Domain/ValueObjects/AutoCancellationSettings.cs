namespace ReservationService.Domain.ValueObjects;

public class AutoCancellationSettings
{
    public bool IsEnabled { get; set; } = true;
    public TimeSpan? CancellationTimeout { get; set; }
}



