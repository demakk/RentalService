namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerUserProfileId { get; set; }
    public decimal FullPrice { get; set; }
    public decimal Deposit { get; set; }
}