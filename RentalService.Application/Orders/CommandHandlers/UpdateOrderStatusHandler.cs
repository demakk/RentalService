using Dapper;
using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Orders.CommandHandlers;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatus, OperationResult>
{
    private readonly DapperContext _ctx;
    private readonly OperationResult _result;

    public UpdateOrderStatusHandler(DapperContext ctx)
    {
        _ctx = ctx;
        _result = new OperationResult();
    }

    public async Task<OperationResult> Handle(UpdateOrderStatus request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.Connect();
            connection.Open();

            Order.ValidateOrderStatus(request.Status);
            var updateResponse = await connection
                .ExecuteAsync(Queries.UpdateOrderManagerIdAndState,
                    new { ManagerId = request.UserProfileId, Status = request.Status, OrderId = request.OrderId });

            if (updateResponse != 1)
            {
                _result.AddError(ErrorCode.NotFound, OrderErrorMessages.OrderNotFound);
                return _result;
            }
        }
        catch (OrderNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e =>
            {
                _result.AddValidationError(e);
            });
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private class Queries
    {
        public const string UpdateOrderManagerIdAndState = "UPDATE Orders" +
                                                           " SET ManagerId = @ManagerId, Status = @Status" +
                                                           " WHERE Id = @OrderId";
    }
}