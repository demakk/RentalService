using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Application.ShoppingCart.Commands;

public class DeleteCartRecordCommand : IRequest<GenericOperationResult<Cart>>
{
    public Guid CartId { get; set; }
    public Guid UserProfileId { get; set; }
}