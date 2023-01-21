using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalService.Application.UserProfiles.Queries;
using RentalService.Dal;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.QueryHandlers;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfile>
{
    private readonly DataContext _ctx;

    public GetUserProfileByIdQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<UserProfile> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _ctx.UserProfiles
            .Include(up => up.UserBasicInfo)
            .ThenInclude(bi => bi.UserContacts)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        return profile;
    }
}