using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetOrderItemsByIdQuery : IRequest<OperationResult<List<OrderItemLink>>>
{
    public Guid OrderId { get; set; }
}