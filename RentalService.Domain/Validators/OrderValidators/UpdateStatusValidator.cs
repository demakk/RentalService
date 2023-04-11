using FluentValidation;
using RentalService.Domain.Aggregates.Common;

namespace RentalService.Domain.Validators.OrderValidators;

public class UpdateStatusValidator : AbstractValidator<string>
{
    public UpdateStatusValidator()
    {
        //TO DO: ADD check if the value is logical
        RuleFor(x => x)
            .NotNull().WithMessage("Status cannot be null");
    }
}