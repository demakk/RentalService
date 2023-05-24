using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class
    GetOrderItemsByIdQueryHandler : IRequestHandler<GetOrderItemsByIdQuery, GenericOperationResult<Order>>
{
    private readonly DataContext _ctx;

    public GetOrderItemsByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GenericOperationResult<Order>> Handle(GetOrderItemsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<Order>();

        if (request.OrderId != request.RequesterId)
        {
            result.AddError(ErrorCode.UnauthorizedOrderView, "Only an owner of the post or the manager can see all ");
        }
        try
        {
            var order = await _ctx.Orders
                .Include(o => o.OrderItemLinks)
                .ThenInclude(ol => ol.Item)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId,
                    cancellationToken: cancellationToken);

            result.Payload = order;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}