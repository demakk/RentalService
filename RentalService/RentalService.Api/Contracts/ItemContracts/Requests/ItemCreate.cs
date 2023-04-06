namespace RentalService.Api.Contracts.ItemContracts.Requests;

public class ItemCreate
{
    public string ItemCategoryId { get;  set; }
    public string ManufacturerId { get;  set; }
    public decimal InitialPrice { get;  set; }
    public string Description { get;  set; }
}