namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderResponse
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
}