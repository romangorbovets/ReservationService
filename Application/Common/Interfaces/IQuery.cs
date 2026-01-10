using MediatR;

namespace ReservationService.Application.Common.Interfaces;

/// <summary>
/// Базовый интерфейс для запросов
/// </summary>
/// <typeparam name="TResponse">Тип возвращаемого значения</typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}



