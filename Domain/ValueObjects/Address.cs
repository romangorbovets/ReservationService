namespace ReservationService.Domain.ValueObjects;

/// <summary>
/// Value Object для представления адреса
/// </summary>
public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string? State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }

    private Address() { } // Для EF Core

    public Address(string street, string city, string postalCode, string country, string? state = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
        State = state;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Address other)
            return false;

        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               PostalCode == other.PostalCode &&
               Country == other.Country;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, PostalCode, Country);
    }

    public override string ToString()
    {
        var parts = new List<string> { Street, City };
        if (!string.IsNullOrWhiteSpace(State))
            parts.Add(State);
        parts.Add(PostalCode);
        parts.Add(Country);
        return string.Join(", ", parts);
    }

    public static bool operator ==(Address? left, Address? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Address? left, Address? right)
    {
        return !Equals(left, right);
    }
}







