using System.ComponentModel.DataAnnotations;
using FluentValidation;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Validators.ItemValidators;

public class ItemValidator : AbstractValidator<Item>
{
    public ItemValidator()
    {
        RuleFor(i => i.Description)
            .NotEmpty().WithMessage("Item description cannot be empty")
            .MaximumLength(300).WithMessage("Maximum length of item description is 300 characters");
    }
}