using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.Specifications;

namespace ReservationService.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetAsync(ISpecification<Reservation> specification, CancellationToken cancellationToken = default)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        if (specification.Criteria is null)
        {
            throw new ArgumentException("Specification criteria cannot be null", nameof(specification));
        }

        try
        {
            return await _context.Reservations
                .Where(specification.Criteria)
                .FirstOrDefaultAsync(cancellationToken);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
        {
            throw new InvalidOperationException($"Database error: {dbEx.Message}", dbEx);
        }
        catch (Npgsql.NpgsqlException npgsqlEx)
        {
            throw new InvalidOperationException($"PostgreSQL connection error: {npgsqlEx.Message}. Please check your database connection settings.", npgsqlEx);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error executing specification query: {ex.Message}", ex);
        }
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