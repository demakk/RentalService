namespace RentalService.Domain.Aggregates.ItemAggregates;

public class Manufacturer
{
    public Guid Id { get; private set; }
    public int CountryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    private readonly List<Item> _items = new List<Item>();
    public IEnumerable<Item> Items => _items;
}