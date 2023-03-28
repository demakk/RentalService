using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GenericOperationResult<Order>>
{
    private readonly DataContext _ctx;

    public GetOrderByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<GenericOperationResult<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<Order>();

        try
        {
            // var order = await _ctx.OrderItemLinks
            //     .Include(ol => ol.OrderId)
            //     .FirstOrDefaultAsync(ol => ol.OrderId == request.Id);
            
            var order = await _ctx.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken: cancellationToken);
            
            
            if (order is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(OrderErrorMessages.OrderNotFound, request.Id));
                return result;
            }

            result.Payload = order;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}