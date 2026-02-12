using FluentValidation;

namespace ReservationService.Application.Features.Reservations.Commands.ConfirmReservation;

public class ConfirmReservationCommandValidator : AbstractValidator<ConfirmReservationCommand>
{
    public ConfirmReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation ID is required");
    }
}