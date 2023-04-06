using Dapper;
using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;

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

            var response = await connection.QueryAsync<decimal>(Queries.CalculateMoneyToReturn, new{request.OrderId});

            if (!response.Any())
            {
                _result.AddError(ErrorCode.NotFound, OrderErrorMessages.OrderNotFound);
                return _result;
            }

            //await connection.ExecuteAsync(Queries.UpdateOrderStatus, new { OrderId = request.OrderId });
            
            _result.Payload = response.First();
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private class Queries
    {
        public const string sthelese = "SELECT o.TotalPrice / " +
                                       " FROM Orders o" +
                                       " INNER JOIN OrderItemLinks oil ON o.Id = oil.OrderId" +
                                       " INNER JOIN Items i ON i.Id = oil.ItemId" +
                                       " WHERE o.id = @OrderId";
        
        public const string CalculateMoneyToReturn = "SELECT o.TotalPrice * 0.75 " +
                                                     "FROM Orders o " +
                                                     "WHERE o.id = @OrderId";

        public const string UpdateOrderStatus = "UPDATE Orders SET Status = @Status WHERE Id = @OrderId";
    }
}