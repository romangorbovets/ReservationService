using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.ToTable("Restaurants");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.OwnsOne(r => r.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("Street");

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            address.Property(a => a.State)
                .HasMaxLength(100)
                .HasColumnName("State");

            address.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("PostalCode");

            address.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Country");
        });

        builder.OwnsOne(r => r.ContactInfo, contactInfo =>
        {
            contactInfo.Property(ci => ci.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");

            contactInfo.Property(ci => ci.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("PhoneNumber");
        });

        builder.Property(r => r.TimeZone)
            .HasMaxLength(50);

        builder.Property(r => r.OpeningTime)
            .HasColumnType("time");

        builder.Property(r => r.ClosingTime)
            .HasColumnType("time");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt);

        builder.HasMany(r => r.Tables)
            .WithOne(t => t.Restaurant)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Reservations)
            .WithOne(res => res.Restaurant)
            .HasForeignKey(res => res.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}







