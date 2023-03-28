using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Queries;

public class GetAllUserProfilesQuery : IRequest<GenericOperationResult<IEnumerable<UserProfile>>>
{
    
}