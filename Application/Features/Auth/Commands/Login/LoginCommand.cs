using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;