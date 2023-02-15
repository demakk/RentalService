using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Items.CommandHandlers;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, OperationResult<Item>>
{
    private readonly DataContext _ctx;

    public CreateItemCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Item>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Item>();
        try
        {
            var item = Item.CreateItem(request.ItemCategoryId, request.ManufacturerId,
                request.InitialPrice, request.Description);
            _ctx.Items.Add(item);
            await _ctx.SaveChangesAsync(cancellationToken: cancellationToken);
            result.Payload = item;
        }
        catch (ItemNotValidException exception)
        {
            result.IsError = true;
            exception.ValidationErrors.ForEach(er =>
                {
                    var error = new Error
                    {
                        Code = ErrorCode.ValidationError,
                        Message = er
                    };
                    result.Errors.Add(error);
                }
            );
        }
        catch (Exception exception)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = exception.Message
            };
            result.Errors.Add(error);
        }


        return result;
    }
}