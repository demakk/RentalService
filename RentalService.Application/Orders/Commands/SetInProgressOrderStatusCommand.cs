using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Orders.Commands;

public class SetInProgressOrderStatusCommand : IRequest<OperationResult>
{
    public Guid ManagerId { get; set; }
    public Guid OrderId { get; set; }
}