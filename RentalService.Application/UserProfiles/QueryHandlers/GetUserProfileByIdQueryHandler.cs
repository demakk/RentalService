using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.QueryHandlers;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();

        try
        {
            var profile = await _ctx.UserProfiles
                .Include(up => up.UserBasicInfo)
                .ThenInclude(bi => bi.UserContacts)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        
            if (profile is null)
            {
                result.AddError(ErrorCode.NotFound,
                    string.Format(UserProfileErrorMessages.UserProfileNotFound));
                return result;
            }
        
            result.Payload = profile;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}