using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Queries;

public class GetAllItemsQuery : IRequest<GenericOperationResult<List<Item>>>
{
    
}
