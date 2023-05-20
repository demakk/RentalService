using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Application.Services;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Identity.CommandHandlers;

public class RegisterIdentityHandler : IRequestHandler<RegisterIdentity, GenericOperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public RegisterIdentityHandler(DataContext ctx, UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _ctx = ctx;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<GenericOperationResult<string>> Handle(RegisterIdentity request, CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<string>();

        try
        {
            var identityExists = await ValidateProfileExistsAsync(request, result);

            if (identityExists) return result;

            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };
            
            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);
            
            var identityCreationSucceeded = await IdentityCreationSucceededAsync(transaction, result, identityUser, request, cancellationToken);
            
            if (!identityCreationSucceeded) return result;
            
            var profile = await CreateUserProfile(request, identityUser, transaction, cancellationToken);

            await CreateCustomerRole(identityUser, transaction, profile.Id, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            result.Payload = GetJwtString(identityUser, profile);

            return result;
        }
        catch (BasicInfoNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => { result.AddError(ErrorCode.ValidationError, e); });
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }
        return result;
    }


    private async Task<bool> ValidateProfileExistsAsync(RegisterIdentity request, GenericOperationResult<string> result)
    {
        
        var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity is null) return false;
        
        result.AddError(ErrorCode.IdentityUserAlreadyExists, IdentityErrorMessages.UserAlreadyExists);
        return true;
    }

    private async Task<bool> IdentityCreationSucceededAsync(IDbContextTransaction transaction,
        GenericOperationResult<string> result, IdentityUser identityUser, RegisterIdentity request, CancellationToken cancellationToken)
    {
        var creationResult =  await _userManager.CreateAsync(identityUser, request.Password);

        if (creationResult.Succeeded) return true;
        
        foreach (var error in creationResult.Errors)
        {
            result.AddError(ErrorCode.IdentityCreationFailed, error.Description);
        }
        
        await transaction.RollbackAsync(cancellationToken);
        return false;
    }

    private async Task<UserProfile> CreateUserProfile(RegisterIdentity request, IdentityUser identityUser,
        IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            var userProfileGuid = Guid.NewGuid();
            var basicInfo = UserBasicInfo.CreateUserBasicInfo(userProfileGuid, request.FirstName, request.LastName, 
                request.DateOfBirth, request.PassportId, request.Phone, request.Username);

            var profile = UserProfile.CreateUserProfile(basicInfo, identityUser.Id);
            
            _ctx.UserProfiles.Add(profile);
            
            await _ctx.SaveChangesAsync(cancellationToken);

            return profile;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task CreateCustomerRole(IdentityUser identityUser, IDbContextTransaction transaction,
        Guid userProfileId, CancellationToken cancellationToken)
    {
        try
        {
            await _userManager.AddToRoleAsync(identityUser, "Customer");
            var customer = Customer.CreateAndValidateCustomer(userProfileId, 0);
            _ctx.Customers.Add(customer);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
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