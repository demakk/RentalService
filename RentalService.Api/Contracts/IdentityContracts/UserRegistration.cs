using System.ComponentModel.DataAnnotations;
using RentalService.Api.Contracts.UserProfileContracts.Requests;
using RentalService.Domain.Aggregates.UserProfileAggregates;

namespace RentalService.Api.Contracts.IdentityContracts;

public class UserRegistration
{
    
    [Required]
    [EmailAddress]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
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


    public string? PassportId { get; set; }
    public string Phone { get; set; }
}