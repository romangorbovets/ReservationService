using FluentValidation;

namespace ReservationService.Application.Features.Reservations.Commands.GetAvailability;

public class GetAvailabilityCommandValidator : AbstractValidator<GetAvailabilityCommand>
{
    public GetAvailabilityCommandValidator()
    {
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
    }

    private static bool BeInFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}