using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Commands;

public class CreateItemCommand : IRequest<OperationResult<Item>>
{
    public Guid ItemCategoryId { get;  set; }
    public Guid ManufacturerId { get;  set; }
    public decimal InitialPrice { get;  set; }
    public string Description { get;  set; }
}