using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber = null,
    string? Street = null,
    string? City = null,
    string? State = null,
    string? PostalCode = null,
    string? Country = null) : ICommand<Guid>;