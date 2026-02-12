using ReservationService.Application.Common.Interfaces;

namespace ReservationService.Application.Features.Reservations.Commands.GetAvailability;

public class GetAvailabilityCommandHandler : ICommandHandler<GetAvailabilityCommand, List<Guid>>
{
    public Task<List<Guid>> Handle(GetAvailabilityCommand command, CancellationToken cancellationToken = default)
    {
      
        return Task.FromResult(new List<Guid>());
    }
}