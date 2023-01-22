using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.UserProfileContracts.Requests;

public class UserProfileCreate
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get;  set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string LastName { get;  set; }
    
    [Required]
    public DateTime DateOfBirth { get;  set; }
    
    [Required]
    public int CityId { get;  set; }
    
    public string Address { get;  set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public List<UserContactCreate> Contacts { get; set; }
}