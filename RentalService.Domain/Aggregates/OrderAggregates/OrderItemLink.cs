using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class OrderItemLink
{
    public Guid Id { get; private set; }
    public Guid ItemId { get; private set; }
    public Guid OrderId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime? ActualReturnDate { get; private set; }

    //nav properties
    public Item Item { get; private set; }
    public Order Order { get; private set; }
    
    //factory methods
    public static OrderItemLink CreateOrderItemLink(Guid itemId, Guid orderId, DateTime startDate, DateTime endDate)
    {   
        //TO DO: Validate start, end dates
        var orderItemLink = new OrderItemLink()
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            OrderId = orderId,
            StartDate = startDate,
            EndDate = endDate
        };
        return orderItemLink;
    }

    public void UpdateActualReturnDate()
    {
        ActualReturnDate = DateTime.Now;
    }
}