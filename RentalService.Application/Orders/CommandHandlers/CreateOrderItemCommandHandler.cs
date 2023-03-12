using System.Diagnostics.CodeAnalysis;
using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;
using RentalService.Domain.Exceptions;

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
            var orderExists = _ctx.Orders.Any(o => o.Id == request.OrderId);
            if (!orderExists)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(OrderErrorMessages.OrderNotFound, request.OrderId));
                return result;
            }

            var orderItem =
                OrderItemLink.CreateOrderItemLink(request.ItemId, request.OrderId,
                    request.StartDate, request.EndDate);

            _ctx.OrderItemLinks.Add(orderItem);
            await _ctx.SaveChangesAsync(cancellationToken);
            result.Payload = orderItem;
        }
        catch (OrderNotValidException exception)
        {
            exception.ValidationErrors.ForEach(er => result.AddError(ErrorCode.ValidationError, er));
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}