﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.Orders.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.OrderAggregates;

namespace RentalService.Application.Orders.QueryHandlers;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, OperationResult<List<Order>>>
{
    private readonly DataContext _ctx;

    public GetAllOrdersQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<List<Order>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Order>>();

        try
        {
            var orders = await _ctx.Orders.ToListAsync();

            result.Payload = orders;
        }
        catch (Exception exception)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = exception.Message
            };
            result.Errors.Add(error);
        }

        return result;
    }
}