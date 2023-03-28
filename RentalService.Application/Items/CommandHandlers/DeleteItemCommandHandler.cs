using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.CommandHandlers;

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, OperationResult>
{
    private readonly DapperContext _ctx;
    private readonly OperationResult _result;

    public DeleteItemCommandHandler(DapperContext ctx)
    {
        _ctx = ctx;
        _result = new OperationResult();
    }
    public async Task<OperationResult> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.Connect();
            connection.Open();

            var deleteItemResponse = await connection.ExecuteAsync("DELETE FROM Items WHERE Id = @ItemId");
            
            if (deleteItemResponse == 0)
            {
                _result.AddError(ErrorCode.NotFound,
                    string.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return _result;
            }
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }
        return _result;
    }
    
    private class Queries
    {
        public const string DeleteItemById = "DELETE FROM Items WHERE Id = @ItemId";
    }
}