namespace ReservationService.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}