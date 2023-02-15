namespace RentalService.Api;

public static class ApiRoutes
{
    public const string BaseRoute = "api/[controller]";

    public static class UserProfiles
    {
        public const string IdRoute = "{id}";
    }

    public static class Order
    {
        public const string IdRoute = "{id}";

        public const string OrderItemRoute = "{orderId}/item";
        
        public const string OrderItemsByIdRoute = "{orderId}/items";
    }

}