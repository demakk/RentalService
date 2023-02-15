using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OperationResult<Order>>
{
    private readonly DataContext _ctx;

    public GetOrderByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Order>();

        try
        {
            // var order = await _ctx.OrderItemLinks
            //     .Include(ol => ol.OrderId)
            //     .FirstOrDefaultAsync(ol => ol.OrderId == request.Id);
            
            var order = await _ctx.Orders.FirstOrDefaultAsync(o => o.Id == request.Id);
            
            
            if (order is null)
            {
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"Post with id {request.Id} not found"
                };
            
                result.IsError = true;
                result.Errors.Add(error);
                return result;
            }

            result.Payload = order;
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