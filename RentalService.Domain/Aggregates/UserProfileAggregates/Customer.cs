using System.ComponentModel.DataAnnotations;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.UserProfileValidators;

namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class Customer
{
    [Key]
    public Guid UserProfileId { get; private set; }
    public int Discount { get; private set; }

    public static Customer CreateAndValidateCustomer(Guid userProfileId, int discount)
    {
        var validator = new CustomerValidator();

        var objectToValidate = new Customer { UserProfileId = userProfileId, Discount = discount };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;
        
        var exception = new CustomerNotValidException("The customer info is not in valid state");

        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }
}