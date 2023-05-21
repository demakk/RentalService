using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.OrderValidators;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerUserProfileId { get; private set; }
    public Guid? ManagerUserProfileId { get; private set; }
    public Guid StatusId { get; private set; }
    public DateTime DateFrom { get; private set; }
    public DateTime DateTo { get; private set; }
    public DateTime? ActualDateFrom { get; private set; }
    public DateTime? ActualDateTo { get; private set; }
    public decimal TotalSum { get; private set; }
    public int Discount { get; private set; }
    public decimal Deposit { get; private set; }

    //factory methods
    public static Order CreateAndValidateOrder(Guid customerUserProfileId, Guid? managerUserProfileId,
        Guid statusId, DateTime dateFrom, DateTime dateTo, DateTime? actualDateFrom, DateTime? actualDateTo,
        decimal totalSum, int discount, decimal deposit)
    {
        var validator = new OrderValidator();
        var objectToValidate = new Order
        {
            Id = Guid.NewGuid(),
            CustomerUserProfileId = customerUserProfileId,
            ManagerUserProfileId = managerUserProfileId,
            StatusId = statusId, DateFrom = dateFrom, DateTo = dateTo,
            ActualDateFrom = actualDateFrom, ActualDateTo = actualDateTo,
            TotalSum = totalSum, Discount = discount, Deposit = deposit
        };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new OrderNotValidException("Order is not in valid state");
        
        validationResult.Errors.ForEach(e =>
        {
            exception.ValidationErrors.Add(e.ErrorMessage);
        });

        throw exception;
    }

    public static bool ValidateOrderDates(string dateFrom, string dateTo)
    {
        var isDate = DateTime.TryParse(dateFrom, out var dateTimeFrom);
        if (!isDate)
        {
            var exception = new OrderNotValidException("Order is not in valid state");
            exception.ValidationErrors.Add("DateFrom is not a date");
        }

        isDate = DateTime.TryParse(dateTo, out var dateTimeTo);
        if (!isDate)
        {
            var exception = new OrderNotValidException("Order is not in valid state");
            exception.ValidationErrors.Add("DateFrom is not a date");
        }

        return true;
    }

}