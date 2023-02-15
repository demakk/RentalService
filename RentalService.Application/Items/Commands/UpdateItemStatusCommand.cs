using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Commands;

public class UpdateItemStatusCommand : IRequest<OperationResult<Item>>
{
    public Guid Id { get; set; }
}