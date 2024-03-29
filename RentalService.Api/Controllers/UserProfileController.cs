﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.UserProfileContracts.Requests;
using RentalService.Api.Contracts.UserProfileContracts.Responses;
using RentalService.Api.Extensions;
using RentalService.Api.Filters;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Application.UserProfiles.Queries;


namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
[Authorize]
public class UserProfileController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public UserProfileController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileByIdQuery {Id = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }

        var profile = _mapper.Map<UserProfileResponse>(response.Payload);
        return Ok(profile);
    }

    [HttpGet]
    [Route(ApiRoutes.UserProfiles.CurrentIdRoute)]
    public async Task<IActionResult> GetCurrentProfileInfo()
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var query = new GetUserProfileByIdQuery { Id = userProfileId };
        
        var response = await _mediator.Send(query);
        if (response.IsError)
        {
            return HandleErrorResponse(response.Errors);
        }

        var profile = _mapper.Map<UserProfileResponse>(response.Payload);
        return Ok(profile);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserProfiles()
    {
        var query = new GetAllUserProfilesQuery();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return Ok(profiles);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    [ValidateModel]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileCreate profile)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = _mapper.Map<UpdateUserProfileCommand>(profile);
        command.Id = userProfileId;
        var response = await _mediator.Send(command);
        return (response.IsError) ? HandleErrorResponse(response.Errors) : NoContent();
    }
    
}