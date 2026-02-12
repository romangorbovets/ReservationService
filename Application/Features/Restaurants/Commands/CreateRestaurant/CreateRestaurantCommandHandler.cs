using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Entities;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;

namespace ReservationService.Application.Features.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandler : ICommandHandler<CreateRestaurantCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateRestaurantCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken = default)
    {
        var contactInfo = new ContactInfo
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        var address = new Address
        {
            Street = command.Street ?? string.Empty,
            City = command.City ?? string.Empty,
            State = command.State,
            PostalCode = command.PostalCode ?? string.Empty,
            Country = command.Country ?? string.Empty
        };

        var restaurant = new Restaurant
        {
            Name = command.Name,
            Description = command.Description,
            Address = address,
            ContactInfo = contactInfo,
            TimeZone = command.TimeZone,
            OpeningTime = command.OpeningTime,
            ClosingTime = command.ClosingTime,
            IsActive = true
        };

        await _context.Restaurants.AddAsync(restaurant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return restaurant.Id;
    }
}