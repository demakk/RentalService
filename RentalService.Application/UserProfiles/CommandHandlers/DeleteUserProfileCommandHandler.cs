using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Commands;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.CommandHandlers;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public DeleteUserProfileCommandHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        var profileToDelete = await _ctx.UserProfiles
            .Include(up => up.UserBasicInfo)
            .ThenInclude(bi => bi.UserContacts)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (profileToDelete is null)
        {
            var error = new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"User with id {request.Id} not found"
            };
            
            result.IsError = true;
            result.Errors.Add(error);
            return result;
        }

        _ctx.UserProfiles.Remove(profileToDelete);
        await _ctx.SaveChangesAsync(cancellationToken);

        result.Payload = profileToDelete;
        return result;
    }
}