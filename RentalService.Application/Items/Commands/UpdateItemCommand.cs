using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Commands;

public class UpdateItemCommand : IRequest<GenericOperationResult<Item>>
{
    public Guid Id { get; set; }
    public Guid ManufacturerId { get; set; }
    public Guid ItemCategoryId { get; set; }
    public string Title { get; set; }
    public int Amount { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal FullPrice { get; set; }
    public string? Description { get; set; }
}