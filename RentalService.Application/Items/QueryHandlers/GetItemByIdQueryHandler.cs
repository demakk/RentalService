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
                result.AddError(ErrorCode.NotFound,
                    String.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return result;
            }
        
            result.Payload = item;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        return result;
    }
}