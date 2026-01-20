namespace ReservationService.Persistence.Settings;

public class ConnectionStrings
{
    public const string SectionName = "ConnectionStrings";

    public string DefaultConnection { get; set; } = string.Empty;
}

