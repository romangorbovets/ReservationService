using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Repositories;
using ReservationService.Persistence;

namespace ReservationService.Persistence.Repositories;

/// <summary>
/// Реализация репозитория для работы с ресторанами
/// </summary>
public class RestaurantRepository : BaseRepository<Restaurant, Guid>, IRestaurantRepository
{
    public RestaurantRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Restaurant?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}

