using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ItemValidators;

namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Item
{
    public Guid Id { get;  private set; }
    public Guid ItemCategoryId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public decimal InitialPrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public string Description { get; private set; }
    public string ItemStatus { get; private set; }
    
    //nav properties
    public ItemCategory ItemCategory { get; private set; }
    public Manufacturer Manufacturer { get; private set; }
    public Cart Cart { get; set; }
    
    
    //factory methods
    public static Item CreateItem(Guid itemCategoryId, Guid manufacturerId, decimal initialPrice, string description)
    {
        //TO DO: Validate initial price 
        var validator = new ItemValidator();
        
        var itemToValidate = new Item
        {
            Id = Guid.NewGuid(),
            ItemCategoryId = itemCategoryId,
            ManufacturerId = manufacturerId,
            InitialPrice = initialPrice,
            CurrentPrice = initialPrice,
            Description = description,
            ItemStatus = "Available"
        };

        var validationResult = validator.Validate(itemToValidate);

        if (validationResult.IsValid) return itemToValidate;
        
        var exception = new ItemNotValidException("The item is not valid");

        validationResult.Errors.ForEach(error => 
                exception.ValidationErrors.Add(error.ErrorMessage)
            );
        throw exception;
    }

    public static Item ValidateToUpdateItem(Guid id, Guid itemCategoryId, Guid manufacturerId,
        decimal currentPrice, string description)
    {
        var validator = new ItemValidator();
        
        var itemToValidate = new Item
        {
            Id = id,
            ItemCategoryId = itemCategoryId,
            ManufacturerId = manufacturerId,
            CurrentPrice = currentPrice,
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