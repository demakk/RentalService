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
using RentalService.Domain.Exceptions;

namespace RentalService.Application.ShoppingCart.CommandHandlers;

public class CreateCartRecordCommandHandler : IRequestHandler<CreateCartRecordCommand, OperationResult<string>>
{
    
    private readonly IConfiguration _configuration;

    public CreateCartRecordCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    public async Task<OperationResult<string>> Handle(CreateCartRecordCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var cart = Cart.CreateShoppingCartRecord(request.UserProfileId, request.ItemId);

            var newItem = new
            {
                UserProfileId = cart.UserProfileId,
                ItemId = cart.ItemId,
                ClearDate = cart.ClearDate
            };

            string sql = "INSERT INTO ShoppingCarts (UserProfileId, ItemId, ClearDate)" +
                         "VALUES (@UserProfileId, @ItemId, @ClearDate)";

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