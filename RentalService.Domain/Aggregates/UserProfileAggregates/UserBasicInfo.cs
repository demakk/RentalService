using RentalService.Domain.Aggregates.Common;

namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class UserBasicInfo
{
    private List<UserContact> _userContacts = new List<UserContact>();
    public Guid Id { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Guid UserContactId { get; set; }
    public int CityId { get; private set; }
    public string Address { get; private set; }
    public IEnumerable<UserContact> UserContacts => _userContacts;
    
    //foreign key
    public Guid UserProfileId { get; set; }
    
    //nav property
    public UserProfile UserProfile { get; set; }


    public static UserBasicInfo CreateUserBasicInfo(string firstName, string lastName,
        DateTime dateOfBirth, int cityId, string address, List<UserContact> contacts)
    {
        
        return new UserBasicInfo
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            CityId = cityId,
            Address = address,
            _userContacts = contacts
        };
    }


    public void UpdateBasicInfo(UserBasicInfo basicInfo)
    {
        FirstName = basicInfo.FirstName;
        LastName = basicInfo.LastName;
        DateOfBirth = basicInfo.DateOfBirth;
        CityId = basicInfo.CityId;
        Address = basicInfo.Address;
        _userContacts = basicInfo.UserContacts.ToList();
    }

}