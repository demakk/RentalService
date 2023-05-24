using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.UserProfileContracts.Requests;

public class UserProfileCreate
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get;  set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get;  set; }
    
    [Required]
    public DateTime DateOfBirth { get;  set; }

    public string? PassportId { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}