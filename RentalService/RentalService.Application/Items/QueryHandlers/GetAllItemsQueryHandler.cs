using Dapper;
using MediatR;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.QueryHandlers;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, GenericOperationResult<List<Item>>>
{
    private readonly DapperContext _ctx;
    private readonly GenericOperationResult<List<Item>> _result;

    public GetAllItemsQueryHandler(DapperContext ctx)
    {
        _ctx = ctx;
        _result = new GenericOperationResult<List<Item>>();
    }
    
    public async Task<GenericOperationResult<List<Item>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.Connect();
            connection.Open();

            var itemsResponse = await connection.QueryAsync<Item>("SELECT * FROM Items");
            
            _result.Payload = itemsResponse.ToList();
        }
        catch (Exception exception)
        {
            _result.AddUnknownError(exception.Message);
        }
        return _result;
    }
}