using ReservationService.Domain.Common.Interfaces;
using ReservationService.Domain.Entities;

namespace ReservationService.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с клиентами
/// </summary>
public interface ICustomerRepository : IRepository<Customer, Guid>
{
    /// <summary>
    /// Получить клиента по email
    /// </summary>
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование клиента по email
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

