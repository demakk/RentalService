using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.UserProfileValidators;

namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class Manager
{
    public Guid UserProfileId { get; private set; }
    public decimal Salary { get; private  set; }
    public Guid? BossId { get; private  set; }
    public DateTime HireDate { get; private  set; }
    public DateTime? FireDate { get; private  set; }

    public static Manager CreateAndValidateManager(Guid userProfileId, decimal salary, Guid? bossId,
        DateTime hireDate, DateTime? fireDate)
    {
        var validator = new ManagerValidator();

        var objectToValidate = new Manager
        {
            UserProfileId = userProfileId, Salary = salary, BossId = bossId,
            HireDate = hireDate, FireDate = fireDate
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new ManagerNotValidException("Manager is not in a valid state");

        
        validationResult.Errors.ForEach(e =>
        {
            exception.ValidationErrors.Add(e.ErrorMessage);
        });

        throw exception;
    }
}