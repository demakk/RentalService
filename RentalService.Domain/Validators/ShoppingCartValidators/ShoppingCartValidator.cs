using FluentValidation;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Domain.Validators.ShoppingCartValidators;

public class ShoppingCartValidator : AbstractValidator<Cart>
{
    public ShoppingCartValidator()
    {
    }
}