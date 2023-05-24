

namespace RentalService.Api.Contracts.ItemContracts.Responses;

public class ItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Amount { get; set; } 
    public decimal PricePerDay { get; set; }
    public string? Description { get; set; }
    public ItemCategoryResponse ItemCategory { get; set; }
    public ManufacturerResponse Manufacturer { get; set; }
}