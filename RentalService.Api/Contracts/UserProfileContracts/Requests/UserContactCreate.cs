using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.UserProfileContracts.Requests;

public class UserContactCreate
{
    public string Name { get;  set; }
 
    [Required]
    public string Value { get;  set; }
}