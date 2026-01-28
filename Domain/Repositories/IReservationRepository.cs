using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Specifications;

namespace ReservationService.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetAsync(ISpecification<Reservation> specification, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}