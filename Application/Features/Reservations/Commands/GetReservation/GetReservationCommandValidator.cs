using FluentValidation;

namespace ReservationService.Application.Features.Reservations.Commands.GetReservation;

public class GetReservationCommandValidator : AbstractValidator<GetReservationCommand>
{
    public GetReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation ID is required");
    }
}