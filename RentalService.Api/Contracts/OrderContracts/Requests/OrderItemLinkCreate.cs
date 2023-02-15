using System.Runtime.InteropServices;

namespace RentalService.Api.Contracts.OrderContracts.Requests;

public class OrderItemLinkCreate
{

    public string ItemId { get; set; }
    public string OrderId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}