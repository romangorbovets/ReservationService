using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Tables.Commands.CreateTable;

public record CreateTableCommand(
    Guid RestaurantId,
    string TableNumber,
    int Capacity,
    string? Location = null) : ICommand<Guid>;