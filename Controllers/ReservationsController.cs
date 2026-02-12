using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Reservations.Commands.CancelReservation;
using ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;
using ReservationService.Application.Features.Reservations.Commands.CreateReservation;
using ReservationService.Application.Features.Reservations.Commands.GetAvailability;
using ReservationService.Application.Features.Reservations.Commands.GetReservation;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ICommandSender _commandSender;

    public ReservationsController(ICommandSender commandSender)
    {
        _commandSender = commandSender;
    }

    [HttpGet("availability")]
    public async Task<ActionResult<List<Guid>>> GetAvailability(
        [FromQuery] Guid restaurantId,
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime,
        [FromQuery] int numberOfGuests)
    {
        var command = new GetAvailabilityCommand(restaurantId, startTime, endTime, numberOfGuests);
        var result = await _commandSender.Send(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] CreateReservationCommand command)
    {
        var result = await _commandSender.Send(command);
        return StatusCode(201, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetReservation([FromRoute] Guid id)
    {
        var command = new GetReservationCommand(id);
        var result = await _commandSender.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/confirm")]
    public async Task<ActionResult> ConfirmReservation([FromRoute] Guid id)
    {
        var command = new ConfirmReservationCommand(id);
        await _commandSender.Send(command);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> CancelReservation(
        [FromRoute] Guid id,
        [FromBody] CancelReservationRequest? request = null)
    {
        var command = new CancelReservationCommand(id, request?.CancellationReason);
        await _commandSender.Send(command);
        return NoContent();
    }
}

public record CancelReservationRequest(string? CancellationReason);