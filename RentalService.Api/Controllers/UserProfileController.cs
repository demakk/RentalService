using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalService.Api.Contracts.UserProfileContracts.Requests;
using RentalService.Api.Contracts.UserProfileContracts.Responses;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Application.UserProfiles.Queries;


namespace RentalService.Api.Controllers;

[ApiController]
[Route(ApiRoutes.BaseRoute)]
public class UserProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public UserProfileController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreate profile)
    {
        var command = _mapper.Map<CreateUserProfileCommand>(profile);
        var response = await _mediator.Send(command);
        var result = _mapper.Map<UserProfileResponse>(response);
        return CreatedAtAction("CreateUserProfile", new {id = response.Id}, result);
    }
    
    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileByIdQuery {Id = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var profile = _mapper.Map<UserProfileResponse>(response);
        return Ok(profile);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserProfiles()
    {
        var query = new GetAllUserProfilesQuery();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response);
        return Ok(profiles);
    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileCreate profile, string id)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(profile);
        command.Id = Guid.Parse(id);
        var response = await _mediator.Send(command);
        var userProfile = _mapper.Map<UserProfileResponse>(response);
        return Ok(userProfile);
    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        var command = new DeleteUserProfileCommand { Id = Guid.Parse(id) };
        var response = await _mediator.Send(command);
        return NoContent();
    }
}