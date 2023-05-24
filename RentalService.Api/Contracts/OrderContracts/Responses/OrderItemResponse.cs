namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid ItemCategoryId { get; set; }
    public string Title { get; set; }
    public decimal PricePerDay { get; set; }
}