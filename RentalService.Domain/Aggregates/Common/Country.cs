using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Domain.Aggregates.Common;

public class Country
{
    private readonly List<City> _cities = new List<City>();
    public int Id { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<City> Cities => _cities;
    
    private readonly List<Manufacturer> _manufacturers = new List<Manufacturer>();
    public IEnumerable<Manufacturer> Manufacturers => _manufacturers;
    
    //factory methods
    public Country CreateCountry(string name)
    {
        var country = new Country
        {
            Name = name
        };
        return country;
    }
}