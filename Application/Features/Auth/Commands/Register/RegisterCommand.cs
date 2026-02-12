using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<RegisterResponse>;