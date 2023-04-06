using MediatR;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Application.ShoppingCart.Commands;

public class CreateCartRecordCommand : IRequest<GenericOperationResult<string>>
{
    public Guid ItemId { get; set; }
    public Guid UserProfileId { get; set; }
}