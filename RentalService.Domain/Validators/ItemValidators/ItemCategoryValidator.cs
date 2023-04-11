using FluentValidation;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Validators.ItemValidators;

public class ItemCategoryValidator : AbstractValidator<ItemCategory>
{
    public ItemCategoryValidator()
    {
        RuleFor(ic => ic.Name)
            .NotEmpty().WithMessage("Item category name cannot be empty")
            .MaximumLength(50).WithMessage("Item category name cannot consist of more than 50 characters");
    }
}