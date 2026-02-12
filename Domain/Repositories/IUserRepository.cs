using ReservationService.Domain.Entities;
using ReservationService.Domain.Specifications;

namespace ReservationService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetAsync(ISpecification<User> specification, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
}