using System.Reflection;
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
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        

        return services;
    }
}





