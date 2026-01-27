namespace ReservationService.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    protected static bool EqualOperator(Entity left, Entity right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    protected static bool NotEqualOperator(Entity left, Entity right)
    {
        return !EqualOperator(left, right);
    }

    public override bool Equals(object? obj)
    {
<<<<<<< HEAD
        if (obj == null || obj.GetType() != GetType())
=======
        if (obj is null || obj.GetType() != GetType())
>>>>>>> СQRS
            return false;

        if (obj is not Entity other)
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return EqualOperator(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return NotEqualOperator(left, right);
    }
}