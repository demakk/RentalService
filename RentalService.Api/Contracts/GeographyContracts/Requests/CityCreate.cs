namespace RentalService.Api.Contracts.GeographyContracts.Requests;

public class CityCreate
{
    public string Name { get;  set; }
    public int CountryId { get;  set; }
}