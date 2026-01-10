using MediatR;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Specifications;
using ReservationService.Domain.ValueObjects;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

/// <summary>
/// Обработчик команды создания резервации
/// </summary>
public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        // Получаем необходимые сущности через репозитории
        var table = await _unitOfWork.Tables.GetByIdWithDetailsAsync(request.TableId, cancellationToken);
        if (table == null)
            throw new ArgumentException($"Table with id {request.TableId} not found", nameof(request.TableId));

        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            throw new ArgumentException($"Customer with id {request.CustomerId} not found", nameof(request.CustomerId));

        // Проверяем доступность столика через спецификацию
        var timeRange = new TimeRange(request.StartTime, request.EndTime);
        var (isAvailable, reason) = await TableAvailabilitySpecification.CheckAvailabilityAsync(
            _unitOfWork.Tables,
            _unitOfWork.Reservations,
            request.TableId,
            timeRange,
            request.NumberOfGuests,
            excludeReservationId: null,
            cancellationToken);

        if (!isAvailable)
            throw new InvalidOperationException($"Table is not available: {reason}");

        // Создаем резервацию
        var totalPrice = new Money(request.TotalPriceAmount, request.TotalPriceCurrency);
        var autoCancellationSettings = new AutoCancellationSettings(
            request.AutoCancellationEnabled,
            request.AutoCancellationTimeout);

        var reservation = new Reservation(
            request.CustomerId,
            request.TableId,
            request.RestaurantId,
            timeRange,
            request.NumberOfGuests,
            totalPrice,
            autoCancellationSettings,
            request.SpecialRequests);

        // Сохраняем резервацию через репозиторий
        _unitOfWork.Reservations.Add(reservation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}



