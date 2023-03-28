using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Application.Options;
using RentalService.Application.Services;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.Identity.CommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, GenericOperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<GenericOperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<string>();

        try
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Username);

            var validationResult = await ValidateIdentityUserAsync(identityUser, result, request);
            if (!validationResult) return result;

            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.IdentityId == identityUser.Id, cancellationToken: cancellationToken);
            
            result.Payload = GetJwtString(identityUser, profile);
        }
        catch (Exception e)
        {
            result.AddUnknownError(e.Message);
        }
        return result;
    }

    private async Task<bool> ValidateIdentityUserAsync(IdentityUser identityUser,
        GenericOperationResult<string> result, LoginCommand request)
    {
        if (identityUser is null)
        {
            result.AddError(ErrorCode.NotFound, IdentityErrorMessages.IdentityNotFound);
            return false;
        }

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (validPassword) return true;
        {
            result.AddError(ErrorCode.PasswordNotValid, IdentityErrorMessages.IncorrectPassword);
            return false;
        }

    }

    private string GetJwtString(IdentityUser identityUser, UserProfile profile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, identityUser.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, identityUser.Email),
            new("IdentityId", identityUser.Id),
            new("UserProfileId", profile.Id.ToString())
        });

        var securityToken = _identityService.CreateSecurityToken(claimsIdentity);
            
        return _identityService.WriteToken(securityToken);
    }
}