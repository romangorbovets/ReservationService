using ReservationService.Domain.Entities;
<<<<<<< HEAD
using ReservationService.Domain.Specifications;
=======
>>>>>>> main

namespace ReservationService.Domain.Repositories;

public interface IUserRepository
{
<<<<<<< HEAD
    Task<User?> GetAsync(ISpecification<User> specification, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
}
=======
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
}
>>>>>>> main
