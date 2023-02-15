namespace RentalService.Api.Contracts.OrderContracts.Responses;

public class OrderItemLinkResponse
{
    public Guid ItemId { get;  set; }
    public Guid OrderId { get;  set; }
    public DateTime StartDate { get;  set; }
    public DateTime EndDate { get;  set; }
    public DateTime ActualReturnDate { get;  set; }
}