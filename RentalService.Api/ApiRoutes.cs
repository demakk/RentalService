namespace RentalService.Api;

public static class ApiRoutes
{
    public const string BaseRoute = "api/[controller]";

    public static class UserProfiles
    {
        public const string IdRoute = "id";
        public const string CurrentIdRoute = "myId";
    }

    public static class Order
    {
        public const string IdRoute = "{id}";
        
        public const string CancelById = "cancel/{id}";

        public const string OrderItemRoute = "{orderId}/item";
        
        public const string OrderItemsByIdRoute = "{orderId}/items";

        public const string OrdersByStatusNew = "orders/new";
    }

    public static class Item
    {
        public const string IdRoute = "{itemId}";
        public const string Items = "Items";
    }

    public static class Identity
    {
        public const string Login = "login";
        public const string CustomerRegistration = "userRegistation";
        public const string ManagerRegistration = "managerRegistation";
        public const string IdentityById = "{identityUserId}";
    }

    public static class Cart
    {
        public const string CartRecordId = "{cartId}";
    }

}