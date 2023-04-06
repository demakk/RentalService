using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.OrderContracts.Requests;
using RentalService.Api.Contracts.OrderContracts.Responses;
using RentalService.Api.Extensions;
using RentalService.Api.Filters;
using RentalService.Application.Orders.Commands;
using RentalService.Application.Orders.Queries;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public async Task<IActionResult> CreateOrder()
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new CreateOrderCommand { UserProfileId = userProfileId};

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
    public async Task<IActionResult> GetAllOrders()
    {
        var query = new GetAllOrdersQuery();

        var response = await _mediator.Send(query);

        var mapped = _mapper.Map<List<OrderResponse>>(response.Payload);
        return Ok(mapped);
    }
    
    //TO DO: UpdateOrder
    
    //TO DO: DeleteOrder

    [HttpGet]
    [Route(ApiRoutes.Order.OrderItemsByIdRoute)]
    [ValidateGuid("orderId")]
    public async Task<IActionResult> GetAllOrderItemsById(string orderId)
    {
        var query = new GetOrderItemsByIdQuery {OrderId = Guid.Parse(orderId)};

        var response = await _mediator.Send(query);

        var mapped = _mapper.Map<List<OrderItemLinkResponse>>(response.Payload);
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(mapped);
        
    }


    [HttpPatch]
    [Route(ApiRoutes.Order.IdRoute)]
    [ValidateGuid("orderId")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ConfirmOrder(string orderId, [FromBody] OrderStatusUpdate status)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new UpdateOrderStatus
        {
            OrderId = Guid.Parse(orderId), UserProfileId = userProfileId, Status = status.Status
        };

        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        return Ok();
    }


    [HttpPatch]
    [Route(ApiRoutes.Order.CancelById)]
    [Authorize(Roles = "Manager")]
    [ValidateGuid("id")]
    public async Task<IActionResult> CancelOrder(string id)
    {
        var command = new CancelOrderCommand {OrderId = Guid.Parse(id)};
        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        return Ok(response.Payload);
    }
    
}