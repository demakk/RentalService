using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.OrderValidators;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserProfileId { get; private set; }

    public Guid? ManagerId { get; private set; }
    public DateTime? ActualRentDate { get; private set; }
    public decimal? TotalPrice { get; private set; }

    public string State { get; private set; }


    //nav properties
    public UserProfile UserProfile { get; private set; }

    //factory methods
    public static Order CreateOrder(Guid userProfileId)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            UserProfileId = userProfileId
        };
    }

    public void UpdateOrderTotalPrice(decimal price)
    {
        TotalPrice = price;
    }

    public static void ValidateOrderStatus(string status)
    {
        var validator = new UpdateStatusValidator();
        var validationResult = validator.Validate(status);

        if (validationResult.IsValid) return;

        var exception = new OrderNotValidException();
        
        validationResult.Errors.ForEach(r =>
        {
            exception.ValidationErrors.Add(r.ErrorMessage);
        });

        throw exception;

    }



    public void SetActualReturnDate()
    {
        ActualRentDate = DateTime.Now;
    }

}