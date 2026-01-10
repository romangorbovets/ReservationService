using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Domain.Entities;

namespace ReservationService.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(c => c.ContactInfo, contactInfo =>
        {
            contactInfo.Property(ci => ci.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");

            contactInfo.Property(ci => ci.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("PhoneNumber");
        });

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street)
                .HasMaxLength(200)
                .HasColumnName("Street");

            address.Property(a => a.City)
                .HasMaxLength(100)
                .HasColumnName("City");

            address.Property(a => a.State)
                .HasMaxLength(100)
                .HasColumnName("State");

            address.Property(a => a.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("PostalCode");

            address.Property(a => a.Country)
                .HasMaxLength(100)
                .HasColumnName("Country");
        });

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.HasMany(c => c.Reservations)
            .WithOne(r => r.Customer)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}







