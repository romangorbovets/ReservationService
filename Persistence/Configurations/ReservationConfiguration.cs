using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.AggregateRoots;

namespace ReservationService.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        
        builder.Ignore(r => r.DomainEvents);

        builder.OwnsOne(r => r.TimeRange, timeRange =>
        {
            timeRange.Property(tr => tr.StartTime).HasColumnName("StartTime").IsRequired();
            timeRange.Property(tr => tr.EndTime).HasColumnName("EndTime").IsRequired();
        });

        builder.OwnsOne(r => r.TotalPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("TotalPriceAmount").IsRequired();
            money.Property(m => m.Currency).HasColumnName("Currency").IsRequired().HasMaxLength(10);
        });

        builder.OwnsOne(r => r.AutoCancellationSettings, autoCancellation =>
        {
            autoCancellation.Property(ac => ac.IsEnabled).HasColumnName("AutoCancellationEnabled");
            autoCancellation.Property(ac => ac.CancellationTimeout).HasColumnName("CancellationTimeout");
        });

        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Restaurant)
            .WithMany(res => res.Reservations)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.NumberOfGuests)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.SpecialRequests)
            .HasMaxLength(500);

        builder.Property(r => r.Notes)
            .HasMaxLength(1000);

        builder.Property(r => r.CancellationReason)
            .HasMaxLength(500);
    }
}