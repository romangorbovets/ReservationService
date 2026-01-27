using ReservationService.Domain.AggregateRoots;

namespace ReservationService.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}