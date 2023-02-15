using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.CommandHandlers;

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, OperationResult<OrderItemLink>>
{
    private readonly DataContext _ctx;

    public CreateOrderItemCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<OrderItemLink>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<OrderItemLink>();

        try
        {
            var exists = _ctx.Orders.Any(o => o.Id == request.OrderId);
            if (!exists)
            {
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"Order with id {request.OrderId} not found"
                };
            
                result.IsError = true;
                result.Errors.Add(error);
                return result;
            }
            var orderItem =
                OrderItemLink.CreateOrderItemLink(request.ItemId, request.OrderId,
                    request.StartDate, request.EndDate);
            _ctx.OrderItemLinks.Add(orderItem);
            await _ctx.SaveChangesAsync(cancellationToken);  
            result.Payload = orderItem;
        }
        catch (Exception exception)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = exception.Message
            };
            result.Errors.Add(error);
        }

        return result;
    }
}