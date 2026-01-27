<<<<<<< HEAD
using MediatR;
using Microsoft.AspNetCore.Mvc;
=======
using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.Common.Interfaces;
>>>>>>> СQRS
using ReservationService.Application.Features.Auth.Commands.Login;
using ReservationService.Application.Features.Auth.Commands.Register;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
<<<<<<< HEAD
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
=======
    private readonly ICommandSender _commandSender;

    public AuthController(ICommandSender commandSender)
    {
        _commandSender = commandSender;
>>>>>>> СQRS
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
    {
<<<<<<< HEAD
        var result = await _mediator.Send(command);
=======
        var result = await _commandSender.Send(command);
>>>>>>> СQRS
        return StatusCode(201, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
<<<<<<< HEAD
        var result = await _mediator.Send(command);
=======
        var result = await _commandSender.Send(command);
>>>>>>> СQRS
        return Ok(result);
    }
}