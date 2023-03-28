using Dapper;
using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Items.CommandHandlers;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, GenericOperationResult<Item>>
{
    private readonly DapperContext _ctx;
    private readonly GenericOperationResult<Item> _result;

    public UpdateItemCommandHandler(DapperContext ctx)
    {
        _ctx = ctx;
        _result = new GenericOperationResult<Item>();
    }
    
    public async Task<GenericOperationResult<Item>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.Connect();
            connection.Open();

            var item = Item.ValidateToUpdateItem(request.Id, request.ItemCategoryId, request.ManufacturerId,
                request.CurrentPrice, request.Description);

            var entityToUpdate = new
            {
                ItemCategoryId = request.ItemCategoryId, ManufacturerId = request.ManufacturerId,
                CurrentPrice = request.CurrentPrice, Description = request.Description, ItemId = request.Id
            };
            
            var itemResponse = await connection.ExecuteAsync(Queries.UpdateItemById, entityToUpdate);

            if (itemResponse == 0)
            {
                _result.AddError(ErrorCode.NotFound,
                    string.Format(ItemsErrorMessages.ItemNotFound, request.Id));
                return _result;
            }
            
            _result.Payload = item;
        }
        catch (ItemNotValidException e)
        {
            e.ValidationErrors.ForEach(error => _result.AddError(ErrorCode.ValidationError, error));
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }
        
        return _result;
    }
    
    private class Queries
    {
        public const string UpdateItemById 
            = "UPDATE Items SET ItemCategoryId = @ItemCategoryId, ManufacturerId = @ManufacturerId," +
              "CurrentPrice = @CurrentPrice, Description = @Description" +
              " WHERE Id = @ItemId";
    }
}