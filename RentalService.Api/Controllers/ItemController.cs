﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.ItemContracts.Requests;
using RentalService.Api.Contracts.ItemContracts.Responses;
using RentalService.Api.Filters;
using RentalService.Application.Items.Commands;
using RentalService.Application.Items.Queries;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
public class ItemController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ItemController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] ItemCreate item)
    {
        var command = _mapper.Map<CreateItemCommand>(item);
        var response = await _mediator.Send(command);
        var mapped = _mapper.Map<ItemResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) 
            : CreatedAtAction("CreateItem", new {id = response.Payload.Id}, mapped);
    }

    [HttpPut]
    [Route(ApiRoutes.Item.IdRoute)]
    [ValidateModel]
    [ValidateGuid("itemId")]
    public async Task<IActionResult> UpdateItemById([FromBody] ItemUpdate item, string itemId)
    {
        var command = _mapper.Map<UpdateItemCommand>(item);

        command.Id = Guid.Parse(itemId);

        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        return Ok();
    }

    
    //Is likely to be deleted later
    [HttpDelete]
    [Route(ApiRoutes.Item.IdRoute)]
    [ValidateGuid("itemId")]
    public async Task<IActionResult> DeleteItemById(string itemId)
    {
        var command = new DeleteItemCommand { Id = Guid.Parse(itemId) };

        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        return Ok();
    }

    [HttpGet]
    [Route(ApiRoutes.Item.IdRoute)]
    [ValidateGuid("itemId")]
    public async Task<IActionResult> GetItemById(string itemId)
    {
        var query = new GetItemByIdQuery { Id = Guid.Parse(itemId) };

        var response = await _mediator.Send(query);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        var mapped = _mapper.Map<ItemResponse>(response.Payload);
        return Ok(mapped);
    }

    [HttpGet]
    [Route(ApiRoutes.Item.Items)]
    public async Task<IActionResult> GetAllItems()
    {
        var request = new GetAllItemsQuery();

        var response = await _mediator.Send(request);

        var items = _mapper.Map<List<ItemResponse>>(response.Payload);

        return Ok(items);
    }
}