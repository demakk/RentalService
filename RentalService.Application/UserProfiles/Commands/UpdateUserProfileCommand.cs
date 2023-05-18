using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Commands;

public class UpdateUserProfileCommand : IRequest<GenericOperationResult<UserProfile>>
{
    public Guid Id { get; set; }
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public string? PassportId { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}