namespace ReservationService.Domain.ValueObjects;

/// <summary>
/// Value Object для представления контактной информации
/// </summary>
public class ContactInfo
{
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    private ContactInfo() { } // Для EF Core

    public ContactInfo(string email, string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        if (phoneNumber != null && !IsValidPhoneNumber(phoneNumber))
            throw new ArgumentException("Invalid phone number format", nameof(phoneNumber));

        Email = email;
        PhoneNumber = phoneNumber;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Базовая валидация - можно расширить
        return phoneNumber.Length >= 10 && phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')');
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ContactInfo other)
            return false;

        return Email == other.Email && PhoneNumber == other.PhoneNumber;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Email, PhoneNumber);
    }

    public static bool operator ==(ContactInfo? left, ContactInfo? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ContactInfo? left, ContactInfo? right)
    {
        return !Equals(left, right);
    }
}






