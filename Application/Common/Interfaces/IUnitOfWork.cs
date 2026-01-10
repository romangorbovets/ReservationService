namespace ReservationService.Application.Common.Interfaces;

/// <summary>
/// Unit of Work для управления транзакциями и координации работы репозиториев
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Сохранить все изменения
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Зафиксировать транзакцию
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить репозиторий резерваций
    /// </summary>
    Domain.Repositories.IReservationRepository Reservations { get; }

    /// <summary>
    /// Получить репозиторий столиков
    /// </summary>
    Domain.Repositories.ITableRepository Tables { get; }

    /// <summary>
    /// Получить репозиторий клиентов
    /// </summary>
    Domain.Repositories.ICustomerRepository Customers { get; }

    /// <summary>
    /// Получить репозиторий ресторанов
    /// </summary>
    Domain.Repositories.IRestaurantRepository Restaurants { get; }
}

