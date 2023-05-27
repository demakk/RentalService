using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.AutoMapperProfiles.CustomMappings.OrderMappings;
using RentalService.Api.Contracts.OrderContracts.Requests;
using RentalService.Api.Contracts.OrderContracts.Responses;
using RentalService.Api.Extensions;
using RentalService.Api.Filters;
using RentalService.Application.Orders.Commands;
using RentalService.Application.Orders.Queries;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize]
public class OrderController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public OrderController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreate order)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new CreateOrderCommand
        {
            CustomerUserProfileId = userProfileId, DateFrom = order.DateFrom, DateTo = order.DateTo
        };

        var response = await _mediator.Send(command);

        var mapped = _mapper.Map<OrderResponse>(response.Payload);
        
        return response.IsError ? HandleErrorResponse(response.Errors) 
            : CreatedAtAction("CreateOrder", new {id = response.Payload.Id}, mapped);
    }
    
    [HttpGet]
    [Route(ApiRoutes.Order.IdRoute)]
    [ValidateGuid("id")]
    [ValidateModel]
    public async Task<IActionResult> GetOrderById (string id)
    {
        var query = new GetOrderByIdQuery {Id = Guid.Parse(id)};
        var response = await _mediator.Send(query);

        var mapped = _mapper.Map<OrderResponse>(response.Payload);

        return Ok(mapped);
    }
    
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetAllOrders()
    {
        var query = new GetAllOrdersQuery();

        var response = await _mediator.Send(query);

        var mapped = _mapper.Map<List<OrderResponse>>(response.Payload);
        return Ok(mapped);
    }

    [HttpGet]
    [Route(ApiRoutes.Order.OrderItemsByIdRoute)]
    [ValidateGuid("orderId")]
    public async Task<IActionResult> GetAllOrderItemsByOrderId(string orderId)
    {
        var requesterGuid = HttpContext.GetUserProfileIdClaimValue();
        var query = new GetOrderItemsByIdQuery {OrderId = Guid.Parse(orderId), RequesterId = requesterGuid};

        var response = await _mediator.Send(query);

        var mapped = OrderMaps.MapOrderToOrderInfoResponse(response.Payload);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(mapped);
        
    }

    [HttpGet]
    [Route(ApiRoutes.Order.OrdersByStatusNew)]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetNewOrders()
    {
        var query = new GetOrdersByStatusQuery{StatusName = "New"};
        var response = await _mediator.Send(query);

        var mapped = _mapper.Map<List<OrderResponse>>(response.Payload);
        
        return Ok(mapped);
    }


    [HttpPatch]
    [Route(ApiRoutes.Order.SetInProgressStatus)]
    [ValidateGuid("orderId")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> SetInProgressStatus(string orderId)
    {
        var managerUserProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new SetInProgressOrderStatusCommand
            { ManagerId = managerUserProfileId, OrderId = Guid.Parse(orderId) };
        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
    }

    [HttpPatch]
    [Route(ApiRoutes.Order.SetReadyStatus)]
    [ValidateGuid("orderId")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> SetReadyStatus(string orderId)
    {
        var managerUserProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new SetReadyOrderStatusCommand
            { ManagerId = managerUserProfileId, OrderId = Guid.Parse(orderId) };

        var response = await _mediator.Send(command);

        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
    }

    [HttpPatch]
    [Route(ApiRoutes.Order.SetGivenStatus)]
    [ValidateGuid("orderId")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> SetGivenStatus(string orderId)
    {
        var managerUserProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new SetGivenOrderStatusCommand
            { ManagerId = managerUserProfileId, OrderId = Guid.Parse(orderId) };

        var response = await _mediator.Send(command);

        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
    }


    [HttpPost]
    [Route(ApiRoutes.Order.ReturnOrderById)]
    [Authorize(Roles = "Manager")]
    [ValidateGuid("orderId")]
    public async Task<IActionResult> ReturnOrder(string orderId)
    {
        var command = new ReturnOrderCommand { OrderId = Guid.Parse(orderId) };
        var response = await _mediator.Send(command);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    
    [HttpPatch]
    [Route(ApiRoutes.Order.CancelById)]
    [Authorize(Roles = "Manager")]
    [ValidateGuid("orderId")]
    public async Task<IActionResult> CancelOrder(string orderId)
    {
        var managerUserProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new CancelOrderCommand
            {OrderId = Guid.Parse(orderId), ManagerId = managerUserProfileId};
        
        var response = await _mediator.Send(command);

        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }
    
}