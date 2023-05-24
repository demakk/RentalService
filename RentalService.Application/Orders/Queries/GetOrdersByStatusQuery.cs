using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetOrdersByStatusQuery : IRequest<GenericOperationResult<List<Order>>>
{
    public string StatusName { get; set; }
}