using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationService.Persistence;

namespace ReservationService.Persistence;

/// <summary>
/// Методы расширения для регистрации сервисов Persistence слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы Persistence слоя
    /// </summary>
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Регистрация DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Здесь будут регистрироваться репозитории и другие сервисы Persistence слоя
        // Например:
        // services.AddScoped<IReservationRepository, ReservationRepository>();
        // services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}





