using System.Linq.Expressions;

namespace ReservationService.Domain.Common.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория для работы с сущностями
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public interface IRepository<T, TKey> where T : class
{
    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все сущности
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти сущности по условию
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти первую сущность по условию
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование сущности по условию
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить сущность
    /// </summary>
    void Add(T entity);

    /// <summary>
    /// Добавить несколько сущностей
    /// </summary>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Обновить несколько сущностей
    /// </summary>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// Удалить несколько сущностей
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Получить количество сущностей по условию
    /// </summary>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
}

