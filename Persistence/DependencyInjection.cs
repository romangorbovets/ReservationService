using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReservationService.Persistence.Settings;

namespace ReservationService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ConnectionStrings>(
            configuration.GetSection(ConnectionStrings.SectionName));

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionStrings = serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>().Value;
            options.UseNpgsql(connectionStrings.DefaultConnection);
        });

        return services;
    }
}

