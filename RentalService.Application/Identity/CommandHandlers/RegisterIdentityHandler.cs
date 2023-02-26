using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Application.Options;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Identity.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, OperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _ctx = ctx;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<OperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            if (existingIdentity is not null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.IdentityUserAlreadyExists,
                    Message = "Provided username already exists"
                };
                result.Errors.Add(error);
                return result;
            }

            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };


            using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            var creationResult =  await _userManager.CreateAsync(identityUser, request.Password);

            if (!creationResult.Succeeded)
            {
                result.IsError = true;
                foreach (var error in creationResult.Errors)
                {
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.IdentityCreationFailed,
                        Message = error.Description
                    });   
                }
                await transaction.RollbackAsync();
                return result;
            }
            
            var basicInfo = UserBasicInfo.CreateUserBasicInfo(request.FirstName, request.LastName, request.DateOfBirth,
                request.CityId, request.Address, request.Contacts.ToList());

            var profile = UserProfile.CreateUserProfile(basicInfo, identityUser.Id);
            
            try
            {
                _ctx.UserProfiles.Add(profile);
                await _ctx.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new (JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
                    new (JwtRegisteredClaimNames.Email, identityUser.Email),
                    new ("IdentityId", identityUser.Id),
                    new ("UserProfileId", profile.Id.ToString())
                }),
                Expires = DateTime.Now.AddHours(2),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience[0],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Payload = tokenHandler.WriteToken(token);
            return result;
        }
        catch (BasicInfoNotValidException exception)
        {
            result.IsError = true;
            exception.ValidationErrors.ForEach(e =>
            {
                var error = new Error()
                {
                    Code = ErrorCode.ValidationError,
                    Message = e
                };
                result.Errors.Add(error);
            });
        }
        catch (Exception exception)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = exception.Message
            };
            result.Errors.Add(error);
        }

        return result;
    }
}