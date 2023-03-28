namespace RentalService.Api.Contracts.ItemContracts.Requests;

public class ItemUpdate
{
    public string ItemCategoryId { get;  set; }
    public string ManufacturerId { get;  set; }
    public decimal CurrentPrice { get;  set; }
    public string Description { get;  set; }
}