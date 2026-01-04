using Microsoft.Extensions.DependencyInjection;

namespace ReservationService.Application;

/// <summary>
/// Методы расширения для регистрации сервисов Application слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы Application слоя
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Здесь будут регистрироваться сервисы Application слоя
        // Например: репозитории, сервисы приложения, валидаторы и т.д.

        return services;
    }
}





