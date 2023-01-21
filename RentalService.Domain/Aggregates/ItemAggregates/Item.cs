namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Item
{
    public Guid Id { get;  set; }
    public Guid ItemCategoryId { get;  set; }
    public Guid ManufacturerId { get;  set; }
    public decimal InitialPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public string Description { get; set; }
    public ItemStatus ItemStatus { get; set; }
}