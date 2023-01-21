using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Api.Contracts.UserProfileContracts.Requests;

public class UserProfileCreate
{
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public int CityId { get;  set; }
    public string Address { get;  set; }
    public List<UserContactCreate>? Contacts { get; set; }
}