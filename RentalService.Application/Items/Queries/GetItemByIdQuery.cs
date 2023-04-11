using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Queries;

public class GetItemByIdQuery : IRequest<GenericOperationResult<Item>>
{
    public Guid Id { get; set; }
}