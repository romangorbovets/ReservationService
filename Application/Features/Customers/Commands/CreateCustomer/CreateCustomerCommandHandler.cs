using ReservationService.Application.Common.Interfaces;
using ReservationService.Domain.Entities;
using ReservationService.Domain.ValueObjects;
using ReservationService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly ApplicationDbContext _context;

    public CreateCustomerCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
       
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.ContactInfo.Email == command.Email, cancellationToken);

        if (existingCustomer is not null)
        {
            throw new InvalidOperationException($"Customer with email '{command.Email}' already exists");
        }

        var contactInfo = new ContactInfo
        {
            Email = command.Email,
            PhoneNumber = command.PhoneNumber
        };

        Address? address = null;
        if (!string.IsNullOrWhiteSpace(command.Street) || 
            !string.IsNullOrWhiteSpace(command.City) || 
            !string.IsNullOrWhiteSpace(command.PostalCode) || 
            !string.IsNullOrWhiteSpace(command.Country))
        {
            address = new Address
            {
                Street = command.Street ?? string.Empty,
                City = command.City ?? string.Empty,
                State = command.State,
                PostalCode = command.PostalCode ?? string.Empty,
                Country = command.Country ?? string.Empty
            };
        }

        var customer = new Customer(
            command.FirstName,
            command.LastName,
            contactInfo,
            address);

        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}