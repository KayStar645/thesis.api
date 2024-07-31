namespace Core.Application.Common.Constants
{
    public static class CONSTANT_PERMESSION_DEFAULT
    {
        public static List<string> PERMISSIONS = new List<string>
        {
            "order.view",
            "order.change-status",
            "product.view",
            "category.view",
            "coupon.view",
            "payment.view",
        };

        public static List<string> PERMISSIONS_NO_LOGIN = new List<string>
        {
            "product.view",
            "category.view",
        };
    }
}
