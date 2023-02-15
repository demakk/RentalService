using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.CommandHandlers;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, OperationResult<Item>>
{
    private readonly DataContext _ctx;

    public DeleteItemCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<OperationResult<Item>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Item>();
        var item = await _ctx.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken: cancellationToken);
        if (item is null)
        {
            var error = new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"Item with id {request.Id} not found"
            };
            
            result.IsError = true;
            result.Errors.Add(error);
            return result;
        }

        _ctx.Items.Remove(item);
        await _ctx.SaveChangesAsync(cancellationToken);
        result.Payload = item;
        return result;
    }
}