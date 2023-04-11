using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ItemAggregates;

namespace RentalService.Application.Items.Commands;

public class DeleteItemCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}