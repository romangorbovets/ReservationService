using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Restaurants.Commands.CreateRestaurant;
using ReservationService.Persistence;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly ICommandSender _commandSender;
    private readonly ApplicationDbContext _context;

    public RestaurantsController(ICommandSender commandSender, ApplicationDbContext context)
    {
        _commandSender = commandSender;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<RestaurantDto>>> GetRestaurants()
    {
        var restaurants = await _context.Restaurants
            .Where(r => r.IsActive)
            .Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Email = r.ContactInfo.Email,
                PhoneNumber = r.ContactInfo.PhoneNumber,
                City = r.Address.City,
                Country = r.Address.Country
            })
            .ToListAsync();

        return Ok(restaurants);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var result = await _commandSender.Send(command);
        return StatusCode(201, result);
    }
}

public record RestaurantDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}