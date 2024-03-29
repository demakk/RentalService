﻿using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetOrderItemsByIdQuery : IRequest<GenericOperationResult<Order>>
{
    public Guid RequesterId { get; set; }
    public Guid OrderId { get; set; }
}