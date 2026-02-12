using FluentValidation;

namespace ReservationService.Application.Features.Tables.Commands.CreateTable;

public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(x => x.RestaurantId)
            .NotEmpty().WithMessage("Restaurant ID is required");

        RuleFor(x => x.TableNumber)
            .NotEmpty().WithMessage("Table number is required")
            .MaximumLength(50).WithMessage("Table number must not exceed 50 characters");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0")
            .LessThanOrEqualTo(50).WithMessage("Capacity must not exceed 50");

        RuleFor(x => x.Location)
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Location));
    }
}