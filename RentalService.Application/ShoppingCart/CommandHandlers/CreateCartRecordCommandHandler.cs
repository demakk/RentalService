using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.ShoppingCart.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.ShoppingCart.CommandHandlers;

public class CreateCartRecordCommandHandler : IRequestHandler<CreateCartRecordCommand, OperationResult<string>>
{

    private readonly DataContext _ctx;

    public CreateCartRecordCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    

    public async Task<OperationResult<string>> Handle(CreateCartRecordCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var cart = Cart.CreateShoppingCartRecord(request.UserProfileId, request.ItemId);

            string insert = $"INSERT INTO ShoppingCart (UserProfileId, ItemId, ClearDate)" +
                            $" VALUES ({cart.UserProfileId}, {cart.ItemId}, {cart.ClearDate})";

            var fs = FormattableStringFactory.Create(insert);

            var res = _ctx.ShoppingCarts.FromSqlInterpolated(fs);

            
            await _ctx.SaveChangesAsync();
            
            result.Payload = "Probably successful";
            return result;
        }
        catch (ShoppingCartNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e =>
            {
                var error = new Error
                {
                    Message = e,
                    Code = ErrorCode.ValidationError
                };
                result.Errors.Add(error);
            });
        }
        catch (Exception e)
        {
            var error = new Error
            {
                Message = e.Message,
                Code = ErrorCode.UnknownError
            };
            result.Errors.Add(error);
        }

        return result;
    }
}