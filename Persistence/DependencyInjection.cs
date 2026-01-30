using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReservationService.Domain.Repositories;
using ReservationService.Persistence.Interceptors;
using ReservationService.Persistence.Repositories;
using ReservationService.Persistence.Settings;

namespace ReservationService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(
            configuration.GetSection(nameof(DatabaseOptions)));

        services.AddSingleton<UpdateTimestampsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var interceptor = serviceProvider.GetRequiredService<UpdateTimestampsInterceptor>();
            options.UseNpgsql(databaseOptions.ConnectionString)
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}