namespace RentalService.Api.Contracts.ItemContracts.Responses;

public class ItemResponse
{
    public Guid Id { get; set; }
    public decimal CurrentPrice { get;  set; }
    public string Description { get; set; }
}