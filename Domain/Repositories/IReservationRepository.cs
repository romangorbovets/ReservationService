using ReservationService.Domain.AggregateRoots;
<<<<<<< HEAD
using ReservationService.Domain.Specifications;
=======
>>>>>>> main

namespace ReservationService.Domain.Repositories;

public interface IReservationRepository
{
<<<<<<< HEAD
    Task<Reservation?> GetAsync(ISpecification<Reservation> specification, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
=======
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}
>>>>>>> main
