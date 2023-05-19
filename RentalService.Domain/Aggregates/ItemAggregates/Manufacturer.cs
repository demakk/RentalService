using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ItemValidators;

namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Manufacturer
{
    public Guid Id { get; private set; }
    public int CountryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    
    //factory methods
    public Manufacturer CreateManufacturer(int countryId, string name, string description)
    {
        var validator = new ManufacturerValidator();
        
        
        var manufacturerToValidate = new Manufacturer()
        {
            CountryId = countryId,
            Name = name,
            Description = description
        };

        var validationResult = validator.Validate(manufacturerToValidate);

        if (validationResult.IsValid) return manufacturerToValidate;

        var exception = new ItemNotValidException("Item manufacturer is not valid");

        validationResult.Errors.ForEach(error => 
            exception.ValidationErrors.Add(error.ErrorMessage)
        );
        
        throw exception;
    }
}