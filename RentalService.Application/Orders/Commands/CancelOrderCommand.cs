using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Orders.Commands;

public class CancelOrderCommand : IRequest<GenericOperationResult<decimal>>
{
    public Guid OrderId { get; set; }
    public Guid ManagerId { get; set; }
}