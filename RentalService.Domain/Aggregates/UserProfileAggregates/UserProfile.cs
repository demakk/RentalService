using RentalService.Domain.Aggregates.ShoppingCartAggregates;

namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class UserProfile
{
    public Guid Id { get; private set; }
    public string IdentityId { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }
    
    public UserBasicInfo UserBasicInfo { get; set; }
    public IEnumerable<Cart> CartRecords { get; set; }
    
    public static UserProfile CreateUserProfile(UserBasicInfo userBasicInfo, string identityId)
    {
        //TO DO: add validation, error handling strategies, error notification
        return new UserProfile
        {
            Id = userBasicInfo.UserProfileId,
            UserBasicInfo = userBasicInfo,
            IdentityId = identityId,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public void UpdateUserProfile()
    {
        LastModified = DateTime.UtcNow;
    }
}
