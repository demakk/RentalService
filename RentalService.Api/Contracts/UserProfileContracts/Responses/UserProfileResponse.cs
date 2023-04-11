namespace RentalService.Api.Contracts.UserProfileContracts.Responses;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public BasicInfoResponse BasicInfoResponse { get; set; }
    public DateTime DateCreated { get;  set; }
    public DateTime LastModified { get;  set; }
}