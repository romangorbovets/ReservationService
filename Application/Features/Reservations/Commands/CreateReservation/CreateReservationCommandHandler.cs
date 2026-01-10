using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Entities;
using ReservationService.Domain.Specifications;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

/// <summary>
/// Обработчик команды создания резервации
/// </summary>
public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateReservationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        // Получаем необходимые сущности
        var table = await _context.Tables
            .Include(t => t.Restaurant)
            .FirstOrDefaultAsync(t => t.Id == request.TableId, cancellationToken);

        if (table == null)
            throw new ArgumentException($"Table with id {request.TableId} not found", nameof(request.TableId));

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

        if (customer == null)
            throw new ArgumentException($"Customer with id {request.CustomerId} not found", nameof(request.CustomerId));

        // Проверяем доступность столика
        var timeRange = new TimeRange(request.StartTime, request.EndTime);
        var (isAvailable, reason) = TableAvailabilitySpecification.CheckAvailability(
            table,
            timeRange,
            request.NumberOfGuests);

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

        // Сохраняем резервацию
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}


