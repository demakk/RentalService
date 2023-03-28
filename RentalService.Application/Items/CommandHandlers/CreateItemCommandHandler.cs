using Dapper;
using MediatR;
using RentalService.Application.Items.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.ItemAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Items.CommandHandlers;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, GenericOperationResult<Item>>
{
    private readonly DapperContext _ctx;

    public CreateItemCommandHandler(DapperContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<GenericOperationResult<Item>> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<Item>();
        try
        {
            var connection = _ctx.Connect();
            connection.Open();
            
            var item = Item.CreateItem(request.ItemCategoryId, request.ManufacturerId,
                request.InitialPrice, request.Description);

            var entityToInsert = new
            {
                Id = item.Id, ItemCategoryId = item.ItemCategoryId, ManufacturerId = item.ManufacturerId,
                InitialPrice = item.InitialPrice, CurrentPrice = item.CurrentPrice,
                Description = item.Description, ItemStatus = item.ItemStatus
            };

            var res = await connection.ExecuteAsync(Queries.InsertItem, entityToInsert);
            result.Payload = item;
        }
        catch (ItemNotValidException exception)
        {
            exception.ValidationErrors.ForEach(er => { result.AddValidationError(er); });
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        
        return result;
    }
    
    private class Queries
    {
        public const string InsertItem 
            = "INSERT INTO Items (Id, ItemCategoryId, ManufacturerId, InitialPrice, CurrentPrice, Description, ItemStatus)" +
              " VALUES (@Id, @ItemCategoryId, @ManufacturerId, @InitialPrice, @CurrentPrice, @Description, @ItemStatus)";
    }
}