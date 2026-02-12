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
<<<<<<< HEAD
            var dbConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var interceptor = serviceProvider.GetRequiredService<UpdateTimestampsInterceptor>();
            
            
            var connectionString = dbConfiguration.GetConnectionString("DefaultConnection") 
                ?? databaseOptions.ConnectionString;
            
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var host = dbConfiguration["DatabaseOptions:Host"] ?? "localhost";
                var port = dbConfiguration["DatabaseOptions:Port"] ?? "5432";
                var database = dbConfiguration["DatabaseOptions:Database"] ?? "ReservationService";
                var username = dbConfiguration["DatabaseOptions:Username"] ?? "postgres";
                var password = dbConfiguration["DatabaseOptions:Password"] ?? "postgres";
                
                connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Disable";
            }
            
            options.UseNpgsql(connectionString)
=======
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var interceptor = serviceProvider.GetRequiredService<UpdateTimestampsInterceptor>();
            options.UseNpgsql(databaseOptions.ConnectionString)
>>>>>>> main
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}