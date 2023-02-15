using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.QueryHandlers;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, OperationResult<List<Item>>>
{
    private readonly DataContext _ctx;

    public GetAllItemsQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<Item>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Item>>();
        try
        {
            var items = await _ctx.Items.ToListAsync(cancellationToken: cancellationToken);
            result.Payload = items;
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