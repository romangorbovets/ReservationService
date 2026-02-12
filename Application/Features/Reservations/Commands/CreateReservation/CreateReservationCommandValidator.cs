using FluentValidation;

namespace ReservationService.Application.Features.Reservations.Commands.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.TableId)
            .NotEmpty().WithMessage("Table ID is required");

        RuleFor(x => x.RestaurantId)
            .NotEmpty().WithMessage("Restaurant ID is required");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required")
            .Must(BeInFuture).WithMessage("Start time must be in the future");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x.NumberOfGuests)
            .GreaterThan(0).WithMessage("Number of guests must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Number of guests must not exceed 50");

        RuleFor(x => x.TotalPriceAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Total price amount must be greater than or equal to 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be a 3-letter code (e.g., USD, EUR)");

        RuleFor(x => x.SpecialRequests)
            .MaximumLength(500).WithMessage("Special requests must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecialRequests));
    }

    private static bool BeInFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}