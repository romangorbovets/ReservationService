using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(r => r.Id);

        builder.OwnsOne(r => r.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").IsRequired();
            address.Property(a => a.City).HasColumnName("City").IsRequired();
            address.Property(a => a.State).HasColumnName("State");
            address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
            address.Property(a => a.Country).HasColumnName("Country").IsRequired();
        });

        builder.OwnsOne(r => r.ContactInfo, contactInfo =>
        {
            contactInfo.Property(c => c.Email).HasColumnName("Email").IsRequired();
            contactInfo.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber");
        });

        builder.HasMany(r => r.Tables)
            .WithOne(t => t.Restaurant)
            .HasForeignKey(t => t.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Reservations)
            .WithOne(res => res.Restaurant)
            .HasForeignKey(res => res.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}