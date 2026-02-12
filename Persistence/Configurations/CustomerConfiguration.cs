using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street");
            address.Property(a => a.City).HasColumnName("City");
            address.Property(a => a.State).HasColumnName("State");
            address.Property(a => a.PostalCode).HasColumnName("PostalCode");
            address.Property(a => a.Country).HasColumnName("Country");
        });

        builder.OwnsOne(c => c.ContactInfo, contactInfo =>
        {
            contactInfo.Property(ci => ci.Email).HasColumnName("Email").IsRequired();
            contactInfo.Property(ci => ci.PhoneNumber).HasColumnName("PhoneNumber");
        });

        builder.HasMany(c => c.Reservations)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}