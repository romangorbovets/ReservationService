using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.AggregateRoots;
using ReservationService.Domain.Common.Exceptions;
using ReservationService.Domain.Repositories;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext context)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<Guid> Handle(CreateReservationCommand command, CancellationToken cancellationToken = default)
    {
        
        var customerExists = await _context.Customers.AnyAsync(c => c.Id == command.CustomerId, cancellationToken);
        if (!customerExists)
        {
            throw new KeyNotFoundException($"Customer with id '{command.CustomerId}' not found.");
        }

        var tableExists = await _context.Tables.AnyAsync(t => t.Id == command.TableId, cancellationToken);
        if (!tableExists)
        {
            throw new KeyNotFoundException($"Table with id '{command.TableId}' not found.");
        }

        var restaurantExists = await _context.Restaurants.AnyAsync(r => r.Id == command.RestaurantId, cancellationToken);
        if (!restaurantExists)
        {
            throw new KeyNotFoundException($"Restaurant with id '{command.RestaurantId}' not found.");
        }

        var timeRange = new TimeRange
        {
            StartTime = command.StartTime,
            EndTime = command.EndTime
        };

        var totalPrice = new Money
        {
            Amount = command.TotalPriceAmount,
            Currency = command.Currency
        };

        AutoCancellationSettings? autoCancellationSettings = null;
        if (command.AutoCancellationEnabled.HasValue && command.AutoCancellationEnabled.Value)
        {
            autoCancellationSettings = new AutoCancellationSettings
            {
                IsEnabled = true,
                CancellationTimeout = command.CancellationTimeout
            };
        }

        var reservation = new Reservation(
            command.CustomerId,
            command.TableId,
            command.RestaurantId,
            timeRange,
            command.NumberOfGuests,
            totalPrice,
            autoCancellationSettings,
            command.SpecialRequests);

        await _reservationRepository.AddAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}