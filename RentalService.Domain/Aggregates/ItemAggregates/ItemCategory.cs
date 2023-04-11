using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ItemValidators;

namespace RentalService.Domain.Aggregates.ItemAggregates;

public class ItemCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    private readonly List<Item> _items = new();
    
    public IEnumerable<Item> Items => _items;
    
    //factory methods
    public ItemCategory CreateItemCategory(string name)
    {
        var validator = new ItemCategoryValidator();
        
        var itemCategoryToValidate = new ItemCategory
        {
            Name = name
        };

        var validationResult = validator.Validate(itemCategoryToValidate);

        if (validationResult.IsValid) return itemCategoryToValidate;
        
        var exception = new ItemNotValidException("The item is not valid");

        validationResult.Errors.ForEach(error => 
            exception.ValidationErrors.Add(error.ErrorMessage)
        );

        throw exception;
    }
}   