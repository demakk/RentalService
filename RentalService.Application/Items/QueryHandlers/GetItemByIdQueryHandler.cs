using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.QueryHandlers;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, OperationResult<Item>>
{
    private readonly DataContext _ctx;

    public GetItemByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Item>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Item>();
        try
        {
            var item = await _ctx.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
            if (item is null)
            {
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"User with id {request.Id} not found"
                };
            
                result.IsError = true;
                result.Errors.Add(error);
                return result;
            }
        
            result.Payload = item;
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