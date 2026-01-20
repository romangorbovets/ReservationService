using ReservationService.Domain.Enums;

namespace ReservationService.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email, Role role);
}





