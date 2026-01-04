using Microsoft.Extensions.DependencyInjection;

namespace ReservationService.Domain;

/// <summary>
/// Методы расширения для регистрации сервисов Domain слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы Domain слоя
    /// </summary>
    /// <remarks>
    /// Domain слой обычно не требует регистрации сервисов,
    /// так как содержит только доменные сущности, value objects и бизнес-логику.
    /// Этот метод оставлен для будущих расширений (например, доменные сервисы).
    /// </remarks>
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // Domain слой обычно не содержит сервисов, требующих регистрации в DI
        // Здесь могут быть зарегистрированы доменные сервисы, если они появятся в будущем

        return services;
    }
}





