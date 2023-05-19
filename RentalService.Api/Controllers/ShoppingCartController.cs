using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.ShoppingCart.Requests;
using RentalService.Api.Extensions;
using RentalService.Api.Filters;
using RentalService.Application.ShoppingCart.Commands;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize]
public class ShoppingCartController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public ShoppingCartController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateShoppingCartRecord([FromBody] ShoppingCartRecordCreate cartRecord)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreateCartRecordCommand
            {
                UserProfileId = userProfileId,
                ItemId = cartRecord.ItemId
            };
        var response = await _mediator.Send(command);
            return Ok(response);
    }


    [HttpPost]
    [Route(ApiRoutes.Cart.CartRecordId)]
    [ValidateGuid("cartId")]
    [ValidateModel]
    public async Task<IActionResult> UpdateCartRecordDates([FromBody] UpdateCartRecordDates dates,
        string cartId, CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new UpdateCartRecordDatesCommand
        {
            UserProfileId = userProfileId,
            StartDate = dates.StartDate,
            EndDate = dates.EndDate,
            CartId = Guid.Parse(cartId)
        };

        var response = await _mediator.Send(command, cancellationToken);
        if (response.IsError) return HandleErrorResponse(response.Errors);
        return Ok();
    }

    
    [HttpDelete]
    [Route(ApiRoutes.Cart.CartRecordId)]
    [ValidateGuid("cartId")]
    public async Task<IActionResult> RemoveShoppingCartRecord(string cartId)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new DeleteCartRecordCommand { CartId = Guid.Parse(cartId), UserProfileId = userProfileId };

        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);
        
        return Ok("Deleted Successfully");
    }


}