using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.IdentityContracts;
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

        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }
}