using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.ShoppingCart.Requests;

public class UpdateCartRecordDates
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
}