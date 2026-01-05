namespace ReservationService.Domain.Common;

/// <summary>
/// Базовый класс для доменных событий
/// </summary>
public abstract class DomainEvent
{
    public Guid Id { get; private set; }
    public DateTime OccurredOn { get; private set; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}






