using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применяем конфигурации из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

