using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Commands;

public class CreateOrderCommand : IRequest<GenericOperationResult<Order>>
{
    public Guid CustomerUserProfileId { get; set; }
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
}