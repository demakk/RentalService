using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.CommandHandlers;

public class UpdateItemStatusCommandHandler : IRequestHandler<UpdateItemStatusCommand, OperationResult<Item>>
{
    private readonly DataContext _ctx;

    public UpdateItemStatusCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<Item>> Handle(UpdateItemStatusCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Item>();
        try
        {
            var item = await _ctx.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
            if (item is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return result;
            }
        
            item.ChangeItemStatus();

            _ctx.Items.Update(item);
            await _ctx.SaveChangesAsync(cancellationToken);

            result.Payload = item;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return result;
    }
}