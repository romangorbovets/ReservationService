
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReservationService.Persistence;

#nullable disable

namespace ReservationService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReservationService.Domain.AggregateRoots.Reservation", b =>
                {
                    b.Property<Guid>("Id")
<<<<<<< HEAD
                        .ValueGeneratedOnAdd()
=======
>>>>>>> main
                        .HasColumnType("uuid");

                    b.Property<string>("CancellationReason")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("CancelledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Notes")
<<<<<<< HEAD
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");
=======
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");
>>>>>>> main

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("integer");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.Property<string>("SpecialRequests")
<<<<<<< HEAD
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");
=======
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("Status");
>>>>>>> main

                    b.Property<Guid>("TableId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("TableId");

                    b.ToTable("Reservations", (string)null);
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
<<<<<<< HEAD
                        .ValueGeneratedOnAdd()
=======
>>>>>>> main
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
<<<<<<< HEAD
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");
=======
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");
>>>>>>> main

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
<<<<<<< HEAD
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<TimeSpan?>("ClosingTime")
                        .HasColumnType("interval");
=======
                        .HasColumnType("uuid");

                    b.Property<TimeSpan?>("ClosingTime")
                        .HasColumnType("time");
>>>>>>> main

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
<<<<<<< HEAD
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("OpeningTime")
                        .HasColumnType("interval");

                    b.Property<string>("TimeZone")
                        .HasColumnType("text");
=======
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<TimeSpan?>("OpeningTime")
                        .HasColumnType("time");

                    b.Property<string>("TimeZone")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");
>>>>>>> main

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Restaurants", (string)null);
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Table", b =>
                {
                    b.Property<Guid>("Id")
<<<<<<< HEAD
                        .ValueGeneratedOnAdd()
=======
>>>>>>> main
                        .HasColumnType("uuid");

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
<<<<<<< HEAD
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .HasColumnType("text");
=======
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Location")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");
>>>>>>> main

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.Property<string>("TableNumber")
                        .IsRequired()
<<<<<<< HEAD
                        .HasColumnType("text");
=======
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");
>>>>>>> main

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

<<<<<<< HEAD
                    b.ToTable("Tables", (string)null);
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
=======
                    b.HasIndex("RestaurantId", "TableNumber")
                        .IsUnique();

                    b.ToTable("Tables", (string)null);
>>>>>>> main
                });

            modelBuilder.Entity("ReservationService.Domain.AggregateRoots.Reservation", b =>
                {
                    b.HasOne("ReservationService.Domain.Entities.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ReservationService.Domain.Entities.Restaurant", "Restaurant")
                        .WithMany("Reservations")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ReservationService.Domain.Entities.Table", "Table")
                        .WithMany("Reservations")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("ReservationService.Domain.ValueObjects.AutoCancellationSettings", "AutoCancellationSettings", b1 =>
                        {
                            b1.Property<Guid>("ReservationId")
                                .HasColumnType("uuid");

                            b1.Property<TimeSpan?>("CancellationTimeout")
                                .HasColumnType("interval")
<<<<<<< HEAD
                                .HasColumnName("CancellationTimeout");

                            b1.Property<bool>("IsEnabled")
                                .HasColumnType("boolean")
=======
                                .HasColumnName("AutoCancellationTimeout");

                            b1.Property<bool>("IsEnabled")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("boolean")
                                .HasDefaultValue(true)
>>>>>>> main
                                .HasColumnName("AutoCancellationEnabled");

                            b1.HasKey("ReservationId");

<<<<<<< HEAD
                            b1.ToTable("Reservations", (string)null);
=======
                            b1.ToTable("Reservations");

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.OwnsOne("ReservationService.Domain.ValueObjects.Money", "TotalPrice", b1 =>
                        {
                            b1.Property<Guid>("ReservationId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("TotalPriceAmount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)")
                                .HasDefaultValue("USD")
                                .HasColumnName("TotalPriceCurrency");

                            b1.HasKey("ReservationId");

                            b1.ToTable("Reservations");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.OwnsOne("ReservationService.Domain.ValueObjects.TimeRange", "TimeRange", b1 =>
                        {
                            b1.Property<Guid>("ReservationId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("EndTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("EndTime");

                            b1.Property<DateTime>("StartTime")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("StartTime");

                            b1.HasKey("ReservationId");

<<<<<<< HEAD
                            b1.ToTable("Reservations", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.OwnsOne("ReservationService.Domain.ValueObjects.Money", "TotalPrice", b1 =>
                        {
                            b1.Property<Guid>("ReservationId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("TotalPriceAmount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)")
                                .HasColumnName("Currency");

                            b1.HasKey("ReservationId");

                            b1.ToTable("Reservations", (string)null);
=======
                            b1.ToTable("Reservations");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("ReservationId");
                        });

                    b.Navigation("AutoCancellationSettings")
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Restaurant");

                    b.Navigation("Table");

                    b.Navigation("TimeRange")
                        .IsRequired();

                    b.Navigation("TotalPrice")
                        .IsRequired();
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Customer", b =>
                {
                    b.OwnsOne("ReservationService.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("Country");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("State")
                                .HasColumnType("text")
=======
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("State")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("State");

                            b1.Property<string>("Street")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
>>>>>>> main
                                .HasColumnName("Street");

                            b1.HasKey("CustomerId");

<<<<<<< HEAD
                            b1.ToTable("Customers", (string)null);
=======
                            b1.ToTable("Customers");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.OwnsOne("ReservationService.Domain.ValueObjects.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Email")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
                                .HasColumnName("Email");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("text")
=======
                                .HasMaxLength(255)
                                .HasColumnType("character varying(255)")
                                .HasColumnName("Email");

                            b1.Property<string>("PhoneNumber")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
>>>>>>> main
                                .HasColumnName("PhoneNumber");

                            b1.HasKey("CustomerId");

<<<<<<< HEAD
                            b1.ToTable("Customers", (string)null);
=======
                            b1.ToTable("Customers");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Address");

                    b.Navigation("ContactInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Restaurant", b =>
                {
                    b.OwnsOne("ReservationService.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("City");

                            b1.Property<string>("Country")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("Country");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("State")
                                .HasColumnType("text")
=======
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("State")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
>>>>>>> main
                                .HasColumnName("State");

                            b1.Property<string>("Street")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
=======
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
>>>>>>> main
                                .HasColumnName("Street");

                            b1.HasKey("RestaurantId");

<<<<<<< HEAD
                            b1.ToTable("Restaurants", (string)null);
=======
                            b1.ToTable("Restaurants");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsOne("ReservationService.Domain.ValueObjects.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Email")
                                .IsRequired()
<<<<<<< HEAD
                                .HasColumnType("text")
                                .HasColumnName("Email");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("text")
=======
                                .HasMaxLength(255)
                                .HasColumnType("character varying(255)")
                                .HasColumnName("Email");

                            b1.Property<string>("PhoneNumber")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
>>>>>>> main
                                .HasColumnName("PhoneNumber");

                            b1.HasKey("RestaurantId");

<<<<<<< HEAD
                            b1.ToTable("Restaurants", (string)null);
=======
                            b1.ToTable("Restaurants");
>>>>>>> main

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("ContactInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Table", b =>
                {
                    b.HasOne("ReservationService.Domain.Entities.Restaurant", "Restaurant")
                        .WithMany("Tables")
                        .HasForeignKey("RestaurantId")
<<<<<<< HEAD
                        .OnDelete(DeleteBehavior.Cascade)
=======
                        .OnDelete(DeleteBehavior.Restrict)
>>>>>>> main
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Customer", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Restaurant", b =>
                {
                    b.Navigation("Reservations");

                    b.Navigation("Tables");
                });

            modelBuilder.Entity("ReservationService.Domain.Entities.Table", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}