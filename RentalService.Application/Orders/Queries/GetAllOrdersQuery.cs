using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetAllOrdersQuery : IRequest<GenericOperationResult<List<Order>>>
{
    
}