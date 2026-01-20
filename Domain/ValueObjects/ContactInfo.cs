namespace ReservationService.Domain.ValueObjects;

public class ContactInfo : ValueObject
{
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return PhoneNumber ?? string.Empty;
    }
}




