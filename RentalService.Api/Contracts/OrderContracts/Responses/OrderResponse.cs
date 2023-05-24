namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid CustomerUserProfileId { get; set; }
    public DateTime DateFrom { get; set; }          
    public decimal TotalSum { get; set; }
    public decimal Deposit { get; set; }
}