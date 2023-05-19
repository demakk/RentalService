using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ItemValidators;

namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Item
{
    public Guid Id { get;  private set; }
    public Guid ItemCategoryId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public string Title { get; private set; }
    public int Amount { get; private set; }
    public decimal PricePerDay { get; private set; }
    public decimal FullPrice { get; private set; }
    public string? Description { get; private set; }
    
    //factory methods
    public static Item CreateItem(string itemCategoryId, string manufacturerId, string title,
        int amount, decimal pricePerDay, decimal fullPrice, string? description)
    {
        //TO DO: Validate initial price 
        var validator = new ItemValidator();
        
        
        var itemCategoryIsValid = Guid.TryParse(itemCategoryId, out var itemCategoryGuid);

        if (!itemCategoryIsValid)
        {
            var ex = new ItemNotValidException("The item is not valid");
            ex.ValidationErrors.Add("Item category id is not in GUID format!");
        }
        var manufacturerIsValid = Guid.TryParse(manufacturerId, out var manufacturerGuid);
        if (!manufacturerIsValid)
        {
            var ex = new ItemNotValidException("The item is not valid");
            ex.ValidationErrors.Add("Manufacturer id is not in GUID format!");
        }
        
        
        var itemToValidate = new Item
        {
            Id = Guid.NewGuid(),
            ItemCategoryId = itemCategoryGuid,
            ManufacturerId = manufacturerGuid,
            Title = title,
            Amount = amount,
            PricePerDay = pricePerDay,
            FullPrice = fullPrice,
            Description = description,
        };

        var validationResult = validator.Validate(itemToValidate);

        if (validationResult.IsValid) return itemToValidate;
        
        var exception = new ItemNotValidException("The item is not valid");

        validationResult.Errors.ForEach(error => 
                exception.ValidationErrors.Add(error.ErrorMessage)
            );
        throw exception;
    }

    public static Item ValidateToUpdateItem(Guid id, Guid itemCategoryId, Guid manufacturerId, string title,
        int amount, decimal pricePerDay, decimal fullPrice, string? description)
    {
        var validator = new ItemValidator();
        
        var itemToValidate = new Item
        {
            Id = id,
            ItemCategoryId = itemCategoryId,
            ManufacturerId = manufacturerId,
            Title = title,
            Amount = amount,
            PricePerDay = pricePerDay,
            FullPrice = fullPrice,
            Description = description
        };

        var validationResult = validator.Validate(itemToValidate);

        if (validationResult.IsValid)
        {
            return itemToValidate;
        }

        var exception = new ItemNotValidException("The item is not valid");

        validationResult.Errors.ForEach(error => 
            exception.ValidationErrors.Add(error.ErrorMessage)
        );
        throw exception;
    }
}