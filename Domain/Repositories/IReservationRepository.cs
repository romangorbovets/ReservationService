using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Common.Interfaces;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с резервациями
/// </summary>
public interface IReservationRepository : IRepository<Reservation, Guid>
{
    /// <summary>
    /// Получить резервацию с включенными связанными сущностями
    /// </summary>
    Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить резервации по идентификатору клиента
    /// </summary>
    Task<IEnumerable<Reservation>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить резервации по идентификатору столика
    /// </summary>
    Task<IEnumerable<Reservation>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить резервации по идентификатору ресторана
    /// </summary>
    Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить активные резервации для столика в указанном временном диапазоне
    /// </summary>
    Task<IEnumerable<Reservation>> GetActiveReservationsForTableAsync(
        Guid tableId,
        TimeRange timeRange,
        Guid? excludeReservationId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить резервации, требующие автоматической отмены
    /// </summary>
    Task<IEnumerable<Reservation>> GetReservationsRequiringAutoCancellationAsync(
        DateTime currentTime,
        CancellationToken cancellationToken = default);
}

