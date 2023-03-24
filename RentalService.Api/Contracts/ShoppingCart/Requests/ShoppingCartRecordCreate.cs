using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.ShoppingCart.Requests;

public class ShoppingCartRecordCreate
{
    [Required]
    public Guid ItemId { get; set; }
}