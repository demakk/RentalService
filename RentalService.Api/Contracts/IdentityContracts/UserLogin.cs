using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.IdentityContracts;

public class UserLogin
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}