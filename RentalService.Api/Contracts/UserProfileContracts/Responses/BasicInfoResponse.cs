using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Api.Contracts.UserProfileContracts.Responses;

public class BasicInfoResponse
{
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }
    public int CityId { get;  set; }
    public string Address { get;  set; }
    public List<UserContactResponse>? UserContacts { get; set; }
}