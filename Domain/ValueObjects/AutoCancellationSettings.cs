namespace ReservationService.Domain.ValueObjects;

public class AutoCancellationSettings : ValueObject
{
    public bool IsEnabled { get; set; } = true;
    public TimeSpan? CancellationTimeout { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IsEnabled;
        yield return CancellationTimeout ?? TimeSpan.Zero;
    }
}