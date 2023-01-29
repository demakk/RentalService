using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserProfileId { get;private set; }
    public DateTime ActualRentDate { get;private set; }
    public decimal TotalPrice { get; private set; }


    //nav properties
    public UserProfile UserProfile { get; private set; }

}