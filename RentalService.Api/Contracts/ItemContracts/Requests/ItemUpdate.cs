namespace RentalService.Api.Contracts.ItemContracts.Requests;

public class ItemUpdate
{
    public Guid ManufacturerId { get; set; }
    public Guid ItemCategoryId { get; set; }
    public string Title { get; set; }
    public int Amount { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal FullPrice { get; set; }
    public string? Description { get; set; }
}