using Microsoft.Extensions.DependencyInjection;

namespace ReservationService.Infrastructure;

/// <summary>
/// Методы расширения для регистрации сервисов Infrastructure слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы Infrastructure слоя
    /// </summary>
    /// <remarks>
    /// Infrastructure слой содержит внешние зависимости и интеграции:
    /// - Внешние API клиенты
    /// - Сервисы отправки email/SMS
    /// - Кэширование
    /// - Логирование
    /// - Файловые сервисы
    /// - И другие инфраструктурные сервисы
    /// </remarks>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Здесь будут регистрироваться инфраструктурные сервисы
        // Например:
        // services.AddHttpClient<IExternalApiClient, ExternalApiClient>();
        // services.AddSingleton<IEmailService, EmailService>();
        // services.AddStackExchangeRedisCache(options => { ... });
        // services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}





