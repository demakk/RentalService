using MediatR;
using RentalService.Application.Models;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Application.UserProfiles.Commands;

public class CreateUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public int CityId { get;  set; }
    public string Address { get;  set; }
    public List<UserContact> Contacts { get; set; }
}