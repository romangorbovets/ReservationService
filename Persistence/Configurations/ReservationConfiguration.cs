using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.CustomerId)
            .IsRequired();

        builder.Property(r => r.TableId)
            .IsRequired();

        builder.Property(r => r.RestaurantId)
            .IsRequired();

        builder.OwnsOne(r => r.TimeRange, timeRange =>
        {
            timeRange.Property(tr => tr.StartTime)
                .IsRequired()
                .HasColumnName("StartTime");

            timeRange.Property(tr => tr.EndTime)
                .IsRequired()
                .HasColumnName("EndTime");
        });

        builder.Property(r => r.NumberOfGuests)
            .IsRequired();

        builder.Property(r => r.Status)
            .HasConversion(
                v => v.Value,
                v => ReservationStatus.FromString(v))
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Status");

        builder.OwnsOne(r => r.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("TotalPriceAmount");

            money.Property(m => m.Currency)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("USD")
                .HasColumnName("TotalPriceCurrency");
        });

        builder.OwnsOne(r => r.AutoCancellationSettings, settings =>
        {
            settings.Property(s => s.IsEnabled)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("AutoCancellationEnabled");

            settings.Property(s => s.CancellationTimeout)
                .HasColumnType("interval")
                .HasColumnName("AutoCancellationTimeout");
        });

        builder.Property(r => r.SpecialRequests)
            .HasMaxLength(1000);

        builder.Property(r => r.Notes)
            .HasMaxLength(2000);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt);

        builder.Property(r => r.ConfirmedAt);

        builder.Property(r => r.CancelledAt);

        builder.Property(r => r.CancellationReason)
            .HasMaxLength(500);

        builder.Property(r => r.CompletedAt);

        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.TableId);
        builder.HasIndex(r => r.RestaurantId);
        builder.HasIndex(r => r.TableId);

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

        builder.Ignore(r => r.DomainEvents);
    }
}







