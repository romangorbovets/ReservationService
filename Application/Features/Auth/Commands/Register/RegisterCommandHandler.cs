using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;
using ReservationService.Persistence;

namespace ReservationService.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly ApplicationDbContext _context;

    public RegisterCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterResponse
        {
            UserId = user.Id,
            Email = user.Email,
            Message = "User registered successfully"
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}




