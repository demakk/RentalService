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
            
            var item = Item.CreateItem(request.ItemCategoryId, request.ManufacturerId, request.Title,
                request.Amount, request.PricePerDay, request.FullPrice, request.Description);

            var entityToInsert = new
            {
                Id = item.Id, ItemCategoryId = item.ItemCategoryId, ManufacturerId = item.ManufacturerId,
                Title = item.Title, Amount = item.Amount, PricePerDay = item.PricePerDay, FullPrice = item.FullPrice,
                Description = item.Description
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
            = "INSERT INTO Items (Id, ItemCategoryId, ManufacturerId, Title, Amount, PricePerDay, FullPrice, Description)" +
              " VALUES (@Id, @ItemCategoryId, @ManufacturerId, @Title, @Amount, @PricePerDay, @FullPrice, @Description)";
    }
}