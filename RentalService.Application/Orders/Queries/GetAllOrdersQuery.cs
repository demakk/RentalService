using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetAllOrdersQuery : IRequest<OperationResult<List<Order>>>
{
    
}