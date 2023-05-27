using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.Orders.CommandHandlers;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, GenericOperationResult<decimal>>
{
    private readonly DapperContext _ctx;
    private readonly GenericOperationResult<decimal> _result;


    public CancelOrderHandler(DapperContext ctx)
    {
        _ctx = ctx;
        _result = new GenericOperationResult<decimal>();
    }

    public async Task<GenericOperationResult<decimal>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.Connect();
            connection.Open();

            var ids = new {OrderId = request.OrderId, ManagerId = request.ManagerId };

            var res = await connection.ExecuteAsync("CancelOrder",
                ids, commandType: CommandType.StoredProcedure);

            if (res < 1)
            {
                _result.AddError(ErrorCode.UnknownError, "Not possible to calculate the price");
                return _result;
            }

            _result.Payload = res;
        }
        catch (SqlException e)
        {
            _result.AddSqlError(e.Message);
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }
    
}