using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Domain.Aggregates.OrderAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Orders.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, GenericOperationResult<Order>>
{
    private readonly string _connectionString;
    private readonly GenericOperationResult<Order> _result;

    public CreateOrderCommandHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DapperString");
        _result = new GenericOperationResult<Order>();
    }
    
    public async Task<GenericOperationResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            Order.ValidateOrderDates(request.DateFrom, request.DateTo);

            var newOrder = new
            {
                CustomerUserProfileId = request.CustomerUserProfileId,
                DateFrom = DateTime.Parse(request.DateFrom), DateTo = DateTime.Parse(request.DateTo)
            };
            var createdOrder = await connection
                .QuerySingleOrDefaultAsync<Order>("InsertNewOrder", newOrder, commandType: CommandType.StoredProcedure);
            _result.Payload = createdOrder;
        }
        catch (OrderNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => { _result.AddValidationError(e); });
        }
        catch (Exception exception)
        {
            _result.AddUnknownError(exception.Message);
        }
        return _result;
    }

    private static class Queries

    {
        public const string GetOrderDetails = "";
        /*public const string GetUserCartRecords = "SELECT * FROM ShoppingCarts" +
                                                         " WHERE UserProfileId = @UserProfileId";
        
        public const string InsertOrder = "INSERT INTO Orders (Id, UserProfileId)" +
                                          " VALUES (@Id, @UserProfileId)";
    
        public const string InsertOrderItem = "INSERT INTO OrderItemLinks (Id, ItemId, OrderId, StartDate, EndDate)" +
                                              " VALUES (@Id, @ItemId, @OrderId, @StartDate, @EndDate)";
    
        public const string CalculateTotalPrice = "SELECT SUM(CurrentPrice / 50 * DATEDIFF(day, sc.StartDate, sc.EndDate))" +
                                                  " FROM ShoppingCarts sc" +
                                                  " INNER JOIN Items i ON sc.ItemId = i.Id" +
                                                  " WHERE sc.UserProfileId = @UserProfileId";
    
        public const string UpdateOrderWithTotalPrice = "UPDATE Orders SET TotalPrice = @TotalPrice" +
                                                        " WHERE Id = @OrderId";
    
        public const string SetItemUnavailable = "UPDATE i SET ItemStatus = 'unavailable'" +
                                                 " FROM Items i INNER JOIN ShoppingCarts sc ON sc.ItemId = i.Id" +
                                                 " WHERE sc.UserProfileid = @UserProfileId";*/
    }
}