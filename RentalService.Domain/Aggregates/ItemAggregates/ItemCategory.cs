namespace RentalService.Domain.Aggregates.ItemAggregates;

public class ItemCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    private readonly List<Item> _items = new();
    
    public IEnumerable<Item> Items => _items;
}   