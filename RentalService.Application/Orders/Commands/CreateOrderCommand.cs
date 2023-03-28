﻿using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Commands;

public class CreateOrderCommand : IRequest<GenericOperationResult<Order>>
{
    public Guid UserProfileId { get; set; }
}