using FluentValidation;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Domain.Validators.OrderValidators;

public class OrderItemLinkValidator : AbstractValidator<OrderItemLink>
{
    public OrderItemLinkValidator()
    {
        
    }
}