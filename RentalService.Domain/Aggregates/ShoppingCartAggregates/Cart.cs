using System.ComponentModel.DataAnnotations;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ShoppingCartValidators;

namespace RentalService.Domain.Aggregates.ShoppingCartAggregates;

public class Cart
{
    public Guid CustomerId { get; private set; }
    public Guid ItemId { get; private set; }
    public int Count { get; private set; }
    public DateTime ClearDate { get; private set; }


    public static Cart CreateShoppingCartRecord(Guid customerId, Guid itemId, int count)
    {
        var validator = new ShoppingCartValidator();
        var objectToValidate = new Cart
        {
            CustomerId = customerId,
            ItemId = itemId,
            Count = count,
            ClearDate = DateTime.Now.AddHours(1)
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new ShoppingCartNotValidException("Shopping cart is not in valid state");
        
        validationResult.Errors.ForEach(e =>
        {
            exception.ValidationErrors.Add(e.ErrorMessage);
        });

        throw exception;
    }
}