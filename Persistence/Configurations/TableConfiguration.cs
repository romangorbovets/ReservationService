using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence.Configurations;

public class TableConfiguration : IEntityTypeConfiguration<Table>
{
    public void Configure(EntityTypeBuilder<Table> builder)
    {
        builder.ToTable("Tables");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.RestaurantId)
            .IsRequired();

        builder.Property(t => t.TableNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Capacity)
            .IsRequired();

        builder.Property(t => t.Location)
            .HasMaxLength(200);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        builder.HasIndex(t => t.RestaurantId);

        builder.HasIndex(t => new { t.RestaurantId, t.TableNumber })
            .IsUnique();

        builder.HasMany(t => t.Reservations)
            .WithOne(r => r.Table)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}






