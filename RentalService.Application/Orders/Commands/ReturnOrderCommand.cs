using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Orders.Commands;

public class ReturnOrderCommand : IRequest<GenericOperationResult<string>>
{
    public Guid OrderId { get; set; }
}