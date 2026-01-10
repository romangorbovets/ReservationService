using ReservationService.Domain.Common.Interfaces;
using ReservationService.Domain.Entities;

namespace ReservationService.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с ресторанами
/// </summary>
public interface IRestaurantRepository : IRepository<Restaurant, Guid>
{
    /// <summary>
    /// Получить ресторан с включенными связанными сущностями
    /// </summary>
    Task<Restaurant?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}

