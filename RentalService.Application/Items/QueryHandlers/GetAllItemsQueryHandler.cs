using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Items.Queries;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.QueryHandlers;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, GenericOperationResult<List<Item>>>
{
    private readonly DataContext _ctx; 
    private readonly GenericOperationResult<List<Item>> _result;

    public GetAllItemsQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
        _result = new GenericOperationResult<List<Item>>();
    }
    
    public async Task<GenericOperationResult<List<Item>>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var res = _ctx.Items.Include(i => i.Manufacturer).Include(i => i.ItemCategory).ToList();
             _result.Payload = res;
        }
        catch (Exception exception)
        {
            _result.AddUnknownError(exception.Message);
        }
        return _result;
    }
}