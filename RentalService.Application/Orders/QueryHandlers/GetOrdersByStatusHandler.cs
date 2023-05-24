using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class GetOrdersByStatusHandler : IRequestHandler<GetOrdersByStatusQuery, GenericOperationResult<List<Order>>>
{
    private readonly DapperContext _ctx;


    public GetOrdersByStatusHandler(DapperContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GenericOperationResult<List<Order>>> Handle(GetOrdersByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<List<Order>>();
        try
        {
            var connection = _ctx.Connect();

            var queryResult = await connection
                .QueryAsync<Guid>("SELECT Id FROM OrderStatuses WHERE Name = @Name",
                    new { Name = request.StatusName });

            if (!queryResult.Any())
            {
                result.AddError(ErrorCode.NotFound, "No records with following status name were found");
                return result;
            }

            var orders = (await connection
                .QueryAsync<Order>("SELECT * FROM Orders o WHERE o.StatusId = @StatusId",
                    new { StatusId = queryResult.First() })).ToList();
            result.Payload = orders;
            return result;
        }
        catch (SqlException e)
        {
            result.AddError(ErrorCode.NotFound, "s");
            return result;
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
            return result;
        }
    }
}