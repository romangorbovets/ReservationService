using ReservationService.Domain.Common.Interfaces;
using ReservationService.Domain.Entities;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Repositories;

/// <summary>
/// Репозиторий для работы со столиками
/// </summary>
public interface ITableRepository : IRepository<Table, Guid>
{
    /// <summary>
    /// Получить столик с включенными связанными сущностями
    /// </summary>
    Task<Table?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить столики ресторана
    /// </summary>
    Task<IEnumerable<Table>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить доступные столики ресторана с указанной вместимостью в указанное время
    /// </summary>
    Task<IEnumerable<Table>> GetAvailableTablesAsync(
        Guid restaurantId,
        TimeRange timeRange,
        int numberOfGuests,
        CancellationToken cancellationToken = default);
}

