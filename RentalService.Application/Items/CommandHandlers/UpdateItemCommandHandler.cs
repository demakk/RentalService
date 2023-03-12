using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Items.CommandHandlers;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, OperationResult<Item>>
{
    private readonly DataContext _ctx;

    public UpdateItemCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Item>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Item>();
        try
        {
            var item = await _ctx.Items.FirstOrDefaultAsync(i => i.Id == request.Id,
                cancellationToken: cancellationToken);
            if (item is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return result;
            }

            item.UpdateItem(request.ItemCategoryId, request.ManufacturerId, request.InitialPrice,
                request.CurrentPrice, request.Description);

            _ctx.Items.Update(item);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = item;
        }
        catch (ItemNotValidException e)
        {
            e.ValidationErrors.ForEach(error => result.AddError(ErrorCode.ValidationError, error));
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        
        return result;
    }
}