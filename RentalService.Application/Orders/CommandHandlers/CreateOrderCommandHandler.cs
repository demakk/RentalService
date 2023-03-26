using System.Data;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Application.ShoppingCart;
using RentalService.Domain.Aggregates.OrderAggregates;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Orders.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OperationResult<Order>>
{
    private readonly string _connectionString;
    private readonly OperationResult<Order> _result;

    public CreateOrderCommandHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DapperString");
        _result = new OperationResult<Order>();
    }
    
    public async Task<OperationResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            
            //Getting all card records associated with the provided UserProfileId
            var cartRecords = await GetCartRecords(connection, request.UserProfileId);
            
            if (!cartRecords.Any())
            {
                _result.AddError(ErrorCode.NotFound, ShoppingCartErrorMessages.CartItemsNotFound);
                return _result;
            }

            var transaction = await connection.BeginTransactionAsync(cancellationToken);
            try
            {
                //Creating Order record
                var order = await CreateOrderRecord(request.UserProfileId, transaction, connection);

                //Getting order's id
                var orderId = order.Id;

                //Create OrderItemLinks
                await InsertOrderItemLinks(cartRecords, orderId, transaction, connection);

                //Calculating total price
                var total = await connection
                    .QueryAsync<decimal>(Queries.CalculateTotalPrice, new {UserProfileId = request.UserProfileId}, transaction);
                
                //Update existing order with totalPrice
                var updateResponse =
                    await connection.ExecuteAsync(Queries.UpdateOrderWithTotalPrice,
                        new {TotalPrice = total, OrderId = orderId }, transaction);

                //Set item status to unavailable
                
                
                await transaction.CommitAsync(cancellationToken);
                _result.Payload = order;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
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

    private async Task<Order> CreateOrderRecord(Guid userProfileId, IDbTransaction transaction, IDbConnection connection)
    {
        var order = Order.CreateOrder(userProfileId);
        var entity = new { Id = order.Id, UserProfileId = userProfileId };
        await connection.ExecuteAsync(Queries.InsertOrder, order, transaction);
        return order;
    }

    private async Task InsertOrderItemLinks(List<Cart> cartRecords, Guid orderId, IDbTransaction transaction, IDbConnection connection)
    {
        foreach (var r in cartRecords)
        {
            var orderItem = OrderItemLink.CreateOrderItemLink(r.ItemId, orderId, r.StartDate.Value, r.EndDate.Value);
            var itemToInsert = new
            {
                Id = orderItem.Id, ItemId = orderItem.ItemId, OrderId = orderItem.OrderId,
                StartDate = orderItem.StartDate, EndDate = orderItem.EndDate
            };
            await connection.ExecuteAsync(Queries.InsertOrderItem, itemToInsert, transaction);
        }
    }

    private async Task<List<Cart>> GetCartRecords(IDbConnection connection, Guid userProfileId)
    {
        var cartRecordsResponse = await connection
            .QueryAsync<Cart>(Queries.GetUserCartRecords, new {UserProfileId = userProfileId});
        return cartRecordsResponse.ToList();
    }

    private static class Queries

    {
        public const string GetUserCartRecords = "SELECT * FROM ShoppingCarts" +
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

        public const string SetItemUnavailable = "UPDATE Items SET ItemStatus = 'available'" +
                                                 "WHERE ItemId = @ItemId";
    }
}