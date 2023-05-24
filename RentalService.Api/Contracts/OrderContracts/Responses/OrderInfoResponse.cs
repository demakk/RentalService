using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderInfoResponse
{
    public Guid Id { get; set; }
    public Guid CustomerUserProfileId { get; set; }
    public decimal TotalSum { get; set; }
    public decimal Deposit { get; set; }
    public List<OrderItemLinkResponse> OrderItemLinksResponse { get; set; } = new();
}