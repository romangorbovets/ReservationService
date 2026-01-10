using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Repositories;
using ReservationService.Persistence.Repositories;

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

        // Регистрация UnitOfWork (Scoped - один экземпляр на HTTP-запрос)
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Регистрация репозиториев через UnitOfWork
        // Репозитории создаются внутри UnitOfWork, но можно также зарегистрировать их напрямую, если нужно
        // В данном случае репозитории доступны через UnitOfWork

        return services;
    }
}







