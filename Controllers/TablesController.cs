using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Tables.Commands.CreateTable;
using ReservationService.Persistence;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ICommandSender _commandSender;
    private readonly ApplicationDbContext _context;

    public TablesController(ICommandSender commandSender, ApplicationDbContext context)
    {
        _commandSender = commandSender;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<TableDto>>> GetTables([FromQuery] Guid? restaurantId = null)
    {
        var query = _context.Tables.Where(t => t.IsActive);

        if (restaurantId.HasValue)
        {
            query = query.Where(t => t.RestaurantId == restaurantId.Value);
        }

        var tables = await query
            .Select(t => new TableDto
            {
                Id = t.Id,
                RestaurantId = t.RestaurantId,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity,
                Location = t.Location
            })
            .ToListAsync();

        return Ok(tables);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTable([FromBody] CreateTableCommand command)
    {
        var result = await _commandSender.Send(command);
        return StatusCode(201, result);
    }
}

public record TableDto
{
    public Guid Id { get; init; }
    public Guid RestaurantId { get; init; }
    public string TableNumber { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public string? Location { get; init; }
}