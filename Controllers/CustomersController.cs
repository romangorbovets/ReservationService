using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Customers.Commands.CreateCustomer;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICommandSender _commandSender;

    public CustomersController(ICommandSender commandSender)
    {
        _commandSender = commandSender;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var result = await _commandSender.Send(command);
        return StatusCode(201, result);
    }
}