namespace RentalService.Api.Contracts.ItemContracts.Requests;

public class ItemCreate
{
    public Guid ItemCategoryId { get;  set; }
    public Guid ManufacturerId { get;  set; }
    public decimal InitialPrice { get;  set; }
    public string Description { get;  set; }
}