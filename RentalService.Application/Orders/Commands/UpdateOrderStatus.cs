using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;

namespace RentalService.Application.Orders.Commands;

public class UpdateOrderStatus : IRequest<OperationResult>
{   
    public Guid OrderId { get; set; }
    public Guid UserProfileId { get; set; }
    public string Status { get; set; }
}