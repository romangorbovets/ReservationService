using FluentValidation;

namespace ReservationService.Application.Features.Reservations.Commands.CancelReservation;

public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation ID is required");
    }
}