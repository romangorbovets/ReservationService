namespace ReservationService.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? State { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State ?? string.Empty;
        yield return PostalCode;
        yield return Country;
    }
}