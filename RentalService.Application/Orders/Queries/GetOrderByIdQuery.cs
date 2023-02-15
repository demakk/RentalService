﻿using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.Queries;

public class GetOrderByIdQuery : IRequest<OperationResult<Order>>
{
    public Guid Id { get; set; }
}