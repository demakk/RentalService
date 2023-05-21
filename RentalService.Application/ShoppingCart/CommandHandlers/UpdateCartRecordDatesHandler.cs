using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.ShoppingCart.Commands;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Application.ShoppingCart.CommandHandlers;

public class UpdateCartRecordDatesHandler : IRequestHandler<UpdateCartRecordDatesCommand, GenericOperationResult<Cart>>
{
    private readonly string _connectionString;
    private readonly GenericOperationResult<Cart> _result;

    public UpdateCartRecordDatesHandler(IConfiguration configuration)
    {   
        _connectionString = configuration.GetConnectionString("DapperString");
        _result = new GenericOperationResult<Cart>();
    }
    
    public async Task<GenericOperationResult<Cart>> Handle(UpdateCartRecordDatesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new {CartId = request.CartId};

            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            
            var res = 
                await connection.QueryAsync<Cart>(Queries.GetCartById,  entity);
        
            if (!res.Any())
            {
                _result.AddError(ErrorCode.NotFound, ShoppingCartErrorMessages.CartItemNotFound);
                return _result;
            }

            var cartRecord = res.First();

            /*cartRecord.AddStartAndEndDates(request.StartDate, request.EndDate);

            var entityToUpdate = new
            {
                StartDate = cartRecord.StartDate,
                EndDate = cartRecord.EndDate,
                CartId = cartRecord.Id
            };*/
                
            /*var resUpdate = await connection.ExecuteAsync(Queries.UpdateCartDates, entityToUpdate);
            _result.Payload = cartRecord;*/
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
        
        public const string UpdateCartDates 
            = "UPDATE ShoppingCarts SET StartDate = @StartDate, EndDate = @EndDate " +
              "WHERE Id = @CartId";
    }
}