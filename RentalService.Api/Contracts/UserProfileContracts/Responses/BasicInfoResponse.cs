using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Api.Contracts.UserProfileContracts.Responses;

public class BasicInfoResponse
{
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public DateTime DateOfBirth { get;  set; }

    public string PassportId { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    
}