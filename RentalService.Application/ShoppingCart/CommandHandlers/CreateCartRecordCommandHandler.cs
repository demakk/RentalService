using System.Runtime.CompilerServices;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.ShoppingCart.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.ShoppingCart.CommandHandlers;

public class CreateCartRecordCommandHandler : IRequestHandler<CreateCartRecordCommand, GenericOperationResult<string>>
{
    
    private readonly IConfiguration _configuration;

    public CreateCartRecordCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public async Task<GenericOperationResult<string>> Handle(CreateCartRecordCommand request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<string>();

        try
        {
            var cart = Cart.CreateShoppingCartRecord(request.CustomerId, request.ItemId, request.Count);

            var newItem = new
            {
                CustomerId = cart.CustomerId,
                ItemId = cart.ItemId,
                Count = cart.Count,
                ClearDate = cart.ClearDate
            };

            string sql = "INSERT INTO ShoppingCarts (CustomerId, ItemId, Count, ClearDate)" +
                         "VALUES (@CustomerId, @ItemId, @Count, @ClearDate)";

            var connection = new SqlConnection(_configuration.GetConnectionString("DapperString"));
            await connection.OpenAsync(cancellationToken);

            var res = await connection.ExecuteAsync(sql, newItem);
            
            await connection.CloseAsync();
            
            result.Payload = "Probably successful";
            return result;
        }
        catch (ShoppingCartNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => result.AddValidationError(e));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }

        return result;
    }
}