using Dapper;
using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.QueryHandlers;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, GenericOperationResult<Item>>
{
    private readonly DapperContext _ctx;

    public GetItemByIdQueryHandler(DapperContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<GenericOperationResult<Item>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<Item>();
        try
        {
            var connection  = _ctx.Connect();
            connection.Open();
            
            var getItemByIdResponse = await connection
                .QueryAsync<Item>("SELECT * FROM Items i WHERE i.Id = @ItemId",
                    new {ItemId = request.Id});

            if (!getItemByIdResponse.Any())
            {
                result.AddError(ErrorCode.NotFound,
                    String.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return result;
            }
        
            result.Payload = getItemByIdResponse.First();
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        return result;
    }
}