using FluentValidation;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Domain.Validators.UserProfileValidators;

public class UserContactValidator : AbstractValidator<UserContact>
{
    public UserContactValidator()
    {
        RuleFor(uc => uc.Value)
            .MinimumLength(5).WithMessage("The contact value cannot have less than 5 characters")
            .MaximumLength(20).WithMessage("The contact value cannot have more than 20 symbols");
    }
}