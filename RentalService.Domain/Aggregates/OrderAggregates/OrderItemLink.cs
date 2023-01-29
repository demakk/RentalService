using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Aggregates.OrderAggregates;

public class OrderItemLink
{
    public Guid Id { get; private set; }
    public Guid ProductCategoryId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public decimal InitialPrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public string Description { get; private set; }
    public ItemStatus Status { get; private set; }
    
    //nav properties
    public Item Item { get; private set; }
    public Order Order { get; private set; }
}