using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Application.Options;
using RentalService.Application.Services;
using RentalService.Dal;

namespace RentalService.Application.Identity.CommandHandlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IOptions<JwtSettings> jwtSettings, IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
        _jwtSettings = jwtSettings.Value;
        
    }

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Username);

            if (identityUser is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"Provided username does not exist"
                };
                result.Errors.Add(error);
                return result;
            }

            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPassword)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.PasswordNotValid,
                    Message = $"Provided password is incorrect"
                };
                result.Errors.Add(error);
                return result;
            }

            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.IdentityId == identityUser.Id);
            
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, identityUser.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, identityUser.Email),
                new("IdentityId", identityUser.Id),
                new("UserProfileId", profile.Id.ToString())
            });

            var securityToken = _identityService.CreateSecurityToken(claimsIdentity);
            
            result.Payload = _identityService.WriteToken(securityToken);
            return result;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}