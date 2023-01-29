namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Item
{
    public Guid Id { get;  set; }
    public Guid ItemCategoryId { get; private set; }
    public Guid ManufacturerId { get; private set; }
    public decimal InitialPrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public string Description { get; private set; }
    public ItemStatus ItemStatus { get; private set; }
    
    //nav properties
    public ItemCategory ItemCategory { get; private set; }
    public Manufacturer Manufacturer { get; private set; }
    
}