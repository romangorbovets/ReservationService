using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;
using ReservationService.Application.Features.Reservations.Commands.CreateReservation;
using ReservationService.Application.Features.Reservations.Queries.GetReservation;
using ReservationService.Application.Features.Reservations.Queries.GetReservations;

namespace ReservationService.Controllers;

/// <summary>
/// Контроллер для работы с резервациями
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Создать резервацию
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateReservation([FromBody] CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservationId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetReservation), new { id = reservationId }, reservationId);
    }

    /// <summary>
    /// Получить резервацию по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetReservation(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetReservationQuery { ReservationId = id };
        var reservation = await _sender.Send(query, cancellationToken);
        return Ok(reservation);
    }

    /// <summary>
    /// Получить список резерваций
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ReservationDto>>> GetReservations(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? tableId,
        [FromQuery] Guid? restaurantId,
        CancellationToken cancellationToken)
    {
        var query = new GetReservationsQuery
        {
            CustomerId = customerId,
            TableId = tableId,
            RestaurantId = restaurantId
        };
        var reservations = await _sender.Send(query, cancellationToken);
        return Ok(reservations);
    }

    /// <summary>
    /// Подтвердить резервацию
    /// </summary>
    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmReservation(Guid id, CancellationToken cancellationToken)
    {
        var command = new ConfirmReservationCommand { ReservationId = id };
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }
}


