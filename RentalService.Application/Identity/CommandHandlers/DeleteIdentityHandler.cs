using System.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Identity.Commands;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles;
using RentalService.Dal;

namespace RentalService.Application.Identity.CommandHandlers;

public class DeleteIdentityHandler : IRequestHandler<DeleteIdentityCommand, GenericOperationResult<bool>>
{
    private readonly DataContext _ctx;
    private GenericOperationResult<bool> _result = new();

    public DeleteIdentityHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GenericOperationResult<bool>> Handle(DeleteIdentityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var identityUser = await _ctx.Users
                .FirstOrDefaultAsync(u => u.Id == request.IdentityUserId.ToString(), cancellationToken: cancellationToken);

            if (identityUser is null)
            {
                _result.AddError(ErrorCode.NotFound, IdentityErrorMessages.IdentityNotFound);
                return _result;
            }

            var userProfile = await _ctx.UserProfiles
                .FirstOrDefaultAsync(p => p.IdentityId == request.IdentityUserId.ToString(), cancellationToken);

            if (userProfile is null)
            {
                _result.AddError(ErrorCode.NotFound, UserProfileErrorMessages.UserProfileNotFound);
                return _result;
            }

            if (userProfile.IdentityId != request.RequesterId.ToString())
            {
                _result.AddError(ErrorCode.UnauthorizedAccountRemoval, IdentityErrorMessages.UnauthorizedAccountRemoval);
                return _result;
            }

            await using var transaction = await _ctx.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _ctx.UserProfiles.Remove(userProfile);
                _ctx.Users.Remove(identityUser);
                await _ctx.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _result.Payload = true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        catch (Exception e)
        {
            _result.AddUnknownError(e.Message);
        }

        return _result;
    }
}