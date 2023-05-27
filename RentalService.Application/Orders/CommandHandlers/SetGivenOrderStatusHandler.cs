using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;

namespace RentalService.Application.Orders.CommandHandlers;

public class SetGivenOrderStatusHandler : IRequestHandler<SetGivenOrderStatusCommand, OperationResult>
{
    private readonly DapperContext _ctx;

    public SetGivenOrderStatusHandler(DapperContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult> Handle(SetGivenOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult();

        try
        {
            var connection = _ctx.Connect();

            var ids = new
                { OrderId = request.OrderId, ManagerId = request.ManagerId };

            var res = await connection.ExecuteAsync("SetGivenStatus", ids, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException e)
        {
            result.AddSqlError(e.Message);
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}