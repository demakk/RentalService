using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Application.ShoppingCart.Commands;

public class UpdateCartRecordDatesCommand : IRequest<OperationResult<Cart>>
{
    public Guid UserProfileId { get; set; }
    public Guid CartId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}