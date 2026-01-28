using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Repositories;
<<<<<<< HEAD
using ReservationService.Domain.Specifications;
=======
>>>>>>> main

namespace ReservationService.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    public async Task<Reservation?> GetAsync(ISpecification<Reservation> specification, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(specification.Criteria)
            .FirstOrDefaultAsync(cancellationToken);
=======
    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
>>>>>>> main
    }

    public async Task<Reservation> AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
        return reservation;
    }

    public Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Update(reservation);
        return Task.CompletedTask;
    }
}