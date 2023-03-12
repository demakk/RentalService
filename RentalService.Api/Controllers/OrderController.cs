using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.OrderContracts.Requests;
using RentalService.Api.Contracts.OrderContracts.Responses;
using RentalService.Api.Filters;
using RentalService.Application.Orders.CommandHandlers;
using RentalService.Application.Orders.Commands;
using RentalService.Application.Orders.Queries;
using RentalService.Application.UserProfiles.Queries;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class OrderController : BaseController
{
    public OrderController(IMapper mapper, IMediator mediator) : base(mapper, mediator)
    {
    }
    
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreate order)
    {
        var command = new CreateOrderCommand { UserProfileId = order.UserProfileId};

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


    [HttpPost]
    [ValidateModel]
    [Route(ApiRoutes.Order.OrderItemRoute)]
    [ValidateGuid("orderId")]
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemLinkCreate orderItem, string orderId)
    {
       //Validate Guids
       var command = new CreateOrderItemCommand
       {
           OrderId = Guid.Parse(orderId),
           ItemId = Guid.Parse(orderItem.ItemId),
           StartDate = orderItem.StartDate,
           EndDate = orderItem.EndDate
       };

       var response = await _mediator.Send(command);
       var mapped = _mapper.Map<OrderItemLinkResponse>(response.Payload);
       
       return response.IsError ? HandleErrorResponse(response.Errors) : Ok(mapped);
    }

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

    //Create endpoints for order and orderItemLink creation
}