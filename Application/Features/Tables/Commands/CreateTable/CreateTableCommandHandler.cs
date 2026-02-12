using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Entities;
using ReservationService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Application.Features.Tables.Commands.CreateTable;

public class CreateTableCommandHandler : ICommandHandler<CreateTableCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateTableCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTableCommand command, CancellationToken cancellationToken = default)
    {
        
        var restaurantExists = await _context.Restaurants.AnyAsync(r => r.Id == command.RestaurantId, cancellationToken);
        if (!restaurantExists)
        {
            throw new KeyNotFoundException($"Restaurant with id '{command.RestaurantId}' not found.");
        }

        var table = new Table
        {
            RestaurantId = command.RestaurantId,
            TableNumber = command.TableNumber,
            Capacity = command.Capacity,
            Location = command.Location,
            IsActive = true
        };

        await _context.Tables.AddAsync(table, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return table.Id;
    }
}