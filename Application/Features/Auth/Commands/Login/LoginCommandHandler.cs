using MediatR;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Common.Services;
using ReservationService.Domain.Repositories;

namespace ReservationService.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var passwordHash = PasswordHasher.HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role);

        return new LoginResponse(token, user.Id, user.Email);
    }
}