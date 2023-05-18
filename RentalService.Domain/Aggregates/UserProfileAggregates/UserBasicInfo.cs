using System.ComponentModel.DataAnnotations;
using RentalService.Domain.Exceptions;
using RentalService.Domain.Validators.UserProfileValidators;

namespace RentalService.Domain.Aggregates.UserProfileAggregates;

public class UserBasicInfo
{
    [Key]
    public Guid UserProfileId { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }

    public string? PassportId { get; private set; }

    public string Phone { get; private set; }
    public string Email { get; private set; }


    //nav property
    public UserProfile UserProfile { get; set; }


    public static UserBasicInfo CreateUserBasicInfo(Guid userProfileId, string firstName, string lastName,
        DateTime dateOfBirth, string? passportId, string phone, string email)
    {
        var validator = new BasicInfoValidator();
        
        var objectToValidate = 
            new UserBasicInfo
            {
                UserProfileId = userProfileId,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                PassportId = passportId,
                Phone = phone,
                Email = email
            };

        var validationResult = validator.Validate(objectToValidate);

        if (validationResult.IsValid) return objectToValidate;

        var exception = new BasicInfoNotValidException("The user profile basic info is not valid");

        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }

        throw exception;
    }


    public void UpdateBasicInfo(UserBasicInfo basicInfo)
    {
        FirstName = basicInfo.FirstName;
        LastName = basicInfo.LastName;
        DateOfBirth = basicInfo.DateOfBirth;
        PassportId = basicInfo.PassportId;
        Phone = basicInfo.Phone;
    }

}