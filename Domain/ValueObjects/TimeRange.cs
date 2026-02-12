namespace ReservationService.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}