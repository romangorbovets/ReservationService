using ReservationService.Application.Common.Interfaces;
using ReservationService.Application.Common.Services;
using ReservationService.Domain.Common.Exceptions;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.Specifications;

namespace ReservationService.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        var specification = new UserSpecification(command.Email);
        var existingUser = await _userRepository.GetAsync(specification, cancellationToken);

        if (existingUser is not null)
        {
            throw new DuplicateEntityException($"User with email '{command.Email}' already exists");
        }

        var user = new User
        {
            Email = command.Email,
            PasswordHash = PasswordHasher.HashPassword(command.Password),
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterResponse("User registered successfully");
    }
}