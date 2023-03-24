using FluentValidation;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Domain.Validators.ShoppingCartValidators;

public class ShoppingCartValidator : AbstractValidator<Cart>
{
    public ShoppingCartValidator()
    {
        RuleFor(sc => sc.EndDate)
            .GreaterThan(sc => sc.StartDate)
            .WithMessage("Start date cannot be greater than end date");
    }
}