using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid Id { get; set; }
}