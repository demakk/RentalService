using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.Enums;
using RentalService.Application.Models;
using RentalService.Application.UserProfiles.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.QueryHandlers;

public class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, GenericOperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _ctx;

    public GetAllUserProfilesQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GenericOperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new GenericOperationResult<IEnumerable<UserProfile>>();

        try
        {
            var profiles = await _ctx.UserProfiles
                .Include(up => up.UserBasicInfo)
                .ThenInclude(bi => bi.UserContacts)
                .ToListAsync(cancellationToken);
            result.Payload = profiles;
        }
        catch (Exception exception)
        {
            result.AddUnknownError(exception.Message);
        }

        return result;
    }
}