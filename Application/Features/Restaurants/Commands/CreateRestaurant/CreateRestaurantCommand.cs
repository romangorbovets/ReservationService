using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Restaurants.Commands.CreateRestaurant;

public record CreateRestaurantCommand(
    string Name,
    string? Description = null,
    string Email = "",
    string? PhoneNumber = null,
    string? Street = null,
    string? City = null,
    string? State = null,
    string? PostalCode = null,
    string? Country = null,
    string? TimeZone = null,
    TimeSpan? OpeningTime = null,
    TimeSpan? ClosingTime = null) : ICommand<Guid>;