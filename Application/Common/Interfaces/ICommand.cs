using MediatR;

namespace ReservationService.Application.Common.Interfaces;

/// <summary>
/// Базовый интерфейс для команд (без возвращаемого значения)
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// Базовый интерфейс для команд с возвращаемым значением
/// </summary>
/// <typeparam name="TResponse">Тип возвращаемого значения</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}



