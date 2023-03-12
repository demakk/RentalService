using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.ItemContracts.Responses;
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