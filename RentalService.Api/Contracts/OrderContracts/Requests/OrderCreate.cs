using System.ComponentModel.DataAnnotations;

namespace RentalService.Api.Contracts.OrderContracts.Requests;

public class OrderCreate
{
    public string DateFrom { get; set; }
    public string DateTo { get; set; }
}