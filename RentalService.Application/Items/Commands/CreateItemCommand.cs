using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Commands;

public class CreateItemCommand : IRequest<GenericOperationResult<Item>>
{
    public string Title { get; set; }
    public string ItemCategoryId { get;  set; }
    public string ManufacturerId { get;  set; }
    public int Amount { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal FullPrice { get; set; }
    public string? Description { get;  set; }
}