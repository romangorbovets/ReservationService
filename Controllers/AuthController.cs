using Microsoft.AspNetCore.Mvc;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Features.Auth.Commands.Login;
using ReservationService.Application.Features.Auth.Commands.Register;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICommandSender _commandSender;

    public AuthController(ICommandSender commandSender)
    {
        _commandSender = commandSender;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
    {
        var result = await _commandSender.Send(command);
        return StatusCode(201, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        var result = await _commandSender.Send(command);
        return Ok(result);
    }
}