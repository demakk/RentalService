using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.ShoppingCart.Commands;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Application.ShoppingCart.CommandHandlers;

public class DeleteCartRecordHandler : IRequestHandler<DeleteCartRecordCommand, OperationResult<Cart>>
{
    private readonly OperationResult<Cart> _result;
    private readonly string _connectionString;

    public DeleteCartRecordHandler(IConfiguration configuration)
    {
        _result = new OperationResult<Cart>();
        _connectionString = configuration.GetConnectionString("DapperString");
    }
    
    public async Task<OperationResult<Cart>> Handle(DeleteCartRecordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new {CartId = request.CartId };
            
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var getCartResult = 
                await connection.QueryAsync<Cart>(Queries.GetCartById,  entity);
        
            if (!getCartResult.Any())
            {
                _result.AddError(ErrorCode.NotFound, ShoppingCartErrorMessages.CartItemNotFound);
                return _result;
            }

            var cart = getCartResult.First();
            
            var deleteCartResult = await connection.ExecuteAsync(Queries.DeleteCartRecord, entity);

            _result.Payload = cart;
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }

    private class Queries
    {
        public const string GetCartById
            = "SELECT * FROM ShoppingCarts WHERE Id = @CartId";
        
        public const string DeleteCartRecord
            = "DELETE FROM ShoppingCarts WHERE Id = @CartId";
    }
}