using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.IdentityContracts;
using RentalService.Api.Extensions;
using RentalService.Api.Filters;
using RentalService.Application.Identity.Commands;

namespace RentalService.Api.Controllers;


[ApiController]
[Route(ApiRoutes.BaseRoute)]
public class IdentityController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IdentityController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] UserRegistration registration)
    {
        var command = _mapper.Map<RegisterIdentity>(registration);

        var response = await _mediator.Send(command);

        var token = new AuthenticationResult { Token = response.Payload };
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(token);
    }


    [HttpPost]
    [Route(ApiRoutes.Identity.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] UserLogin login)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var response = await _mediator.Send(command);

        var token = new AuthenticationResult { Token = response.Payload };
        
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(token);
    }

    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [ValidateGuid("identityUserId")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAccount(string identityUserId)
    {
        var requesterId = HttpContext.GetIdentityIdClaimValue();

        var command = new DeleteIdentityCommand
        {
            IdentityUserId = Guid.Parse(identityUserId),
            RequesterId = requesterId
        };

        var response = await _mediator.Send(command);

        if (response.IsError) return HandleErrorResponse(response.Errors);

        return NoContent();
    }
}