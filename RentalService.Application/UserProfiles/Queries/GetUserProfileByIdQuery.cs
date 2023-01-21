using System.Data.Common;
using MediatR;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<UserProfile>
{
    public Guid Id { get; set; }
}