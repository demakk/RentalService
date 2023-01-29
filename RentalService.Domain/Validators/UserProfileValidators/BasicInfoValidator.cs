using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.VisualBasic.CompilerServices;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Domain.Validators.UserProfileValidators;

public class BasicInfoValidator : AbstractValidator<UserBasicInfo>
{
    public BasicInfoValidator()
    {
        RuleFor(bi => bi.FirstName)
            .NotNull().WithMessage("First name cannot be null")
            .MinimumLength(3).WithMessage("The first name cannot have less than 3 characters")
            .MaximumLength(50).WithMessage("The first name cannot have more than 50 characters");
        
        RuleFor(bi => bi.LastName)
            .NotNull().WithMessage("Last name cannot be null")
            .MinimumLength(3).WithMessage("The Last name cannot have less than 3 characters")
            .MaximumLength(50).WithMessage("The Last name cannot have more than 50 characters");

        RuleFor(bi => bi.DateOfBirth)
            .NotNull().WithMessage("Date of birth cannot be null")
            .InclusiveBetween(new DateTime(DateTime.Now.AddYears(-100).Ticks),
                new DateTime(DateTime.Now.AddYears(-18).Ticks))
            .WithMessage("Age has to be between 18 and 100");

        RuleFor(bi => bi.Address)
            .NotNull().WithMessage("Address cannot be null");

        RuleFor(bi => bi.UserContacts.ToList().Count)
            .GreaterThanOrEqualTo(2).WithMessage("A person must have at least 2 ways to contact them");

        RuleForEach(bi => bi.UserContacts)
            .Where(x => x.Name == "Email")
            .Must(contact => Regex.IsMatch(contact.Value, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\." +
                                                          "[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+" +
                                                          "))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
            .WithMessage("The email address is in the wrong state");

        RuleForEach(bi => bi.UserContacts)
            .Where(x => x.Name == "PhoneNumber")
            .Must(c => c.Value.Length == 12 && c.Value.StartsWith("380"));
    }

    
}