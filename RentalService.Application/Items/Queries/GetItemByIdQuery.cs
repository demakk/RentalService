using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Queries;

public class GetItemByIdQuery : IRequest<OperationResult<Item>>
{
    public Guid Id { get; set; }
}