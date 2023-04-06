using System.Data.Common;
using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<GenericOperationResult<UserProfile>>
{
    public Guid Id { get; set; }
}