using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class GetOrderItemsByIdQueryHandler : IRequestHandler<GetOrderItemsByIdQuery, OperationResult<List<OrderItemLink>>>
{
    private readonly DataContext _ctx;

    public GetOrderItemsByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<OrderItemLink>>> Handle(GetOrderItemsByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<OrderItemLink>>();
        try
        {
            var orderItems = await _ctx.OrderItemLinks
                .Where(ol => ol.OrderId == request.OrderId)
                .ToListAsync(cancellationToken: cancellationToken);

            result.Payload = orderItems;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}