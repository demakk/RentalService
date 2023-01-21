using MediatR;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Queries;

public class GetAllUserProfilesQuery : IRequest<IEnumerable<UserProfile>>
{
    
}