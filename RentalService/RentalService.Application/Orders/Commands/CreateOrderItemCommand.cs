using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Commands;

public class CreateOrderItemCommand : IRequest<GenericOperationResult<OrderItemLink>>
{
    public Guid ItemId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}