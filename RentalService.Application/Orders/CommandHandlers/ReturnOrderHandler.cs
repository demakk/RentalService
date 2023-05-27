using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Orders.CommandHandlers;

public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, GenericOperationResult<string>>
{
    private readonly DapperContext _ctx;

    public ReturnOrderHandler(DapperContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GenericOperationResult<string>> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<string>();

        try
        {
            var connection = _ctx.Connect();
            var sql = "EXEC ReturnOrder @OrderId";
            var res = await connection
                .QueryAsync<decimal>(sql, new {OrderId = request.OrderId});

            if (!res.Any()) result.AddError(ErrorCode.UnknownError, "No result was yielded");
            result.Payload = res.First().ToString();
        }
        catch (SqlException e)
        {
            result.AddError(ErrorCode.UnknownError, e.Message);
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}