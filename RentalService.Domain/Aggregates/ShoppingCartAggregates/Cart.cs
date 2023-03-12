using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.ShoppingCartValidators;

namespace RentalService.Domain.Aggregates.ShoppingCartAggregates;

public class Cart
{
        public Guid UserProfileId { get; private set; }
        public Guid ItemId { get; private set; }
        public DateTime? StartDate { get; private  set; }
        public DateTime? EndDate { get; private  set; }
        public DateTime ClearDate { get; private  set; }

    public static Cart CreateShoppingCartRecord(Guid userProfileId, Guid itemId)
    {
        return new Cart
        {
            UserProfileId = userProfileId,
            ItemId = itemId,
            ClearDate = DateTime.Now.AddHours(1),
            StartDate = null,
            EndDate = null
        };
    }

    public void AddStartAndEndDates(DateTime startDate, DateTime endDate)
    {
        var validator = new ShoppingCartValidator();

        StartDate = startDate;
        EndDate = endDate;

        var validationResult = validator.Validate(this);

        if (validationResult.IsValid) return;

        var exception = new ShoppingCartNotValidException("Shopping cart params are not valid");

        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }
    
}