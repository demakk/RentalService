using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class OrderItemLink
{
    public Guid ItemId { get; private set; }
    public Guid OrderId { get; private set; }

    public int Count { get; private set; }
    public decimal Price { get; private set; }
    
    public Item Item { get; set; }

    //factory methods
    public static OrderItemLink CreateOrderItemLink(Guid itemId, Guid orderId, DateTime startDate, DateTime endDate)
    {   
        //TO DO: Validate start, end dates
        var orderItemLink = new OrderItemLink()
        {
            ItemId = itemId,
            OrderId = orderId,
        };
        return orderItemLink;
    }


}