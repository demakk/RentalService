using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.ShoppingCart.Requests;
using RentalService.Application.ShoppingCart.Commands;

namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


public class ShoppingCartController : BaseController
{
    public ShoppingCartController(IMapper mapper, IMediator mediator) : base(mapper, mediator)
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> CreateShoppingCartRecord([FromBody] ShoppingCartRecordCreate cartRecord)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

            var userProfileId = identity?.FindFirst("UserProfileId")?.Value;

            var command = new CreateCartRecordCommand
            {
                UserProfileId = Guid.Parse(userProfileId),
                ItemId = cartRecord.ItemId
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        
    }
}