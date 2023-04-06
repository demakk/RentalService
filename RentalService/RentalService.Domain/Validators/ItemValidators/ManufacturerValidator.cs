using FluentValidation;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Validators.ItemValidators;

public class ManufacturerValidator : AbstractValidator<Manufacturer>
{
    public ManufacturerValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Manufacturer name cannot be empty")
            .MaximumLength(30).WithMessage("Manufacturer name cannot have more than 30 characters");
        
        RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Manufacturer description cannot be empty")
            .MaximumLength(300).WithMessage("Manufacturer description cannot have more than 300 characters");
        
    }
}