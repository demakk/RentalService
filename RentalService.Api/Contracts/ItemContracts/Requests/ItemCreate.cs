namespace RentalService.Api.Contracts.ItemContracts.Requests;

public class ItemCreate
{
    public string Title { get; set; }
    public string ItemCategoryId { get;  set; }
    public string ManufacturerId { get;  set; }
    public int Amount { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal FullPrice { get; set; }
    public string? Description { get;  set; }
}