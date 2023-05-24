using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderItemLinkResponse
{
    public int Count { get; set; }
    public OrderItemResponse ItemResponse { get; set; }
}