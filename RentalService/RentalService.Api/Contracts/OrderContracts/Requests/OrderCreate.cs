using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.OrderContracts.Requests;

public class OrderCreate
{
    [Required]
    public Guid UserProfileId { get; set; }
}