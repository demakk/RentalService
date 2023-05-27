using MediatR;
using RentalService.Application.Models;

namespace RentalService.Application.Orders.Commands;

public class SetReadyOrderStatusCommand : IRequest<OperationResult>
{
    public Guid OrderId { get; set; }
    public Guid ManagerId { get; set; }
}