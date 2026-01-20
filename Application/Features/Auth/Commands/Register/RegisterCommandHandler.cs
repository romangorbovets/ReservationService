using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationService.Application.Common.Services;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;

namespace ReservationService.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        try
        {
            await _userRepository.AddAsync(user, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            throw new InvalidOperationException("User with this email already exists", ex);
        }

        return new RegisterResponse("User registered successfully");
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var message = ex.InnerException?.Message ?? string.Empty;
        return message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) ||
               message.Contains("violates unique constraint", StringComparison.OrdinalIgnoreCase);
    }
}