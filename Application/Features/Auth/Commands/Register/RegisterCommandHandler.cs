using MediatR;
using ReservationService.Application.Common.Services;
using ReservationService.Domain.Common.Exceptions;
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
        catch (DuplicateEntityException)
        {
            throw new InvalidOperationException("Registration failed");
        }

        return new RegisterResponse("User registered successfully");
    }
}