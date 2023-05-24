using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;
using RentalService.Domain.Exceptions;

namespace RentalService.Application.Identity.CommandHandlers;

public class RegisterManagerHandler : IRequestHandler<RegisterManagerCommand, GenericOperationResult<string>>
{
    private readonly DataContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;


    public RegisterManagerHandler(DataContext ctx, UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }

    public async Task<GenericOperationResult<string>> Handle(RegisterManagerCommand request,
        CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<string>();

        try
        {
            var manager = Manager.CreateAndValidateManager(request.UserProfileId, request.Salary,
                request.BossId, request.HireDate, request.FireDate);

            if (!await ValidateUserProfileExists(result, manager.UserProfileId)) return result;

            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            var profile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.Id == manager.UserProfileId, cancellationToken);

            var identityUser = await _userManager.FindByIdAsync(profile.IdentityId);
            
            await AddToManagersTable(transaction, manager, cancellationToken);
            
            await _userManager.AddToRoleAsync(identityUser, "Manager");

            await transaction.CommitAsync(cancellationToken);
        }
        catch (ManagerNotValidException exception)
        {
            exception.ValidationErrors.ForEach(e => result.AddValidationError(e));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }

    private async Task<bool> ValidateUserProfileExists(GenericOperationResult<string> result, Guid userProfileId)
    {
        var profile = await _ctx.UserProfiles.FirstOrDefaultAsync(p => p.Id == userProfileId);
        if (profile is not null) return true;
        
        result.AddError(ErrorCode.UserDoesNotExist, IdentityErrorMessages.IdentityNotFound);
        return false;
    }

    private async Task AddToManagersTable(IDbContextTransaction transaction, Manager manager, CancellationToken cancellationToken)
    {
        try
        {
            _ctx.Managers.Add(manager);
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}