﻿using MediatR;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OperationResult<Order>>
{
    private readonly DataContext _ctx;
    public CreateOrderCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Order>();

        try
        {
            var order = Order.CreateOrder(request.UserProfileId);
            _ctx.Orders.Add(order);
            result.Payload = order;
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}