namespace Core.Application.Transforms
{
    public static class Modules
    {
        public const int NameMin = 3;
        public const int NameMax = 190;
        public const int PhoneNumberLength = 10;
        public const int DescriptionMax = 600000;
        public const int AddressMax = 500;
        public const int IdCardLength = 12;
        public const int InternalCodeMin = 3;
        public const int InternalCodeMax = 50;
        public const int PageNumberMin = 1;
        public const int PageSizeMin = 1;
        public const int MaxDescribes = 600000;

        public const string Name = "Name";
        public const string Email = "Email";
        public const string PhoneNumber = "PhoneNumber";
        public const string Gender = "Gender";
        public const string DateOfBirth = "DateOfBirth";
        public const string Address = "Address";
        public const string IdCard = "IdCard";
        public const string InternalCode = "InternalCode";
        public const string Id = "Id";
        public const string PageNumber = "PageNumber";
        public const string PageSize = "PageSize";
        public const string UrlImage = " hình ảnh ";
        public const string UrlFile = " file ";
        public const string Describes = "Describes";

        public static class User
        {
            public const int UserNameMin = 3;
            public const int UserNameMax = 50;
            public const int PasswordMin = 3;
            public const int PasswordMax = 50;

            public const string Module = "User";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string UserName = "UserName";
            public const string Password = "Password";
            public const string ConfirmPassword = "ConfirmPassword";
        }

        public static class Role
        {
            public const string Module = "Role";
            public const string Description = "Description";
        }

        public static class UserRole
        {
            public const string UserId = "UserId";
            public const string RoleId = "RoleId";
        }

        public static class UserPermission
        {
            public const string UserId = "UserId";
            public const string PermissionId = "RoleId";
        }

        public static class Permission
        {
            public const string Module = "Permission";
        }

        public static class Staff
        {
            public const int Year = 16;

            public const string Module = "Staff";
            public const string Avatar = "Avatar";
            public const string Position = "Position";
            public const string IdCardImage = "IdCardImage";
            public const string PositionId = "PositionId";
        }

        public static class Customer
        {
            public const string Module = "Customer";
        }

        public static class Category
        {
            public const string Module = "Category";

            public const string ParentId = "ParentId";
        }

        public static class Product
        {
            public const int MinPrice = 0;

            public const string Module = "Product";

            public const string ParentId = "ParentId";

            public const string CategoryId = "CategoryId";

            public const string Price = "Price";

            public const string Feature = "Feature";

            public const string Specifications = "Specifications";

            public const string Images = "Images";

            public const string ProductStatus = "ProductStatus";
        }

        public static class SupplierOrder
        {
            public const string Module = "SupplierOrder";

            public const string ReceivingStaffId = "ReceivingStaffId";

            public const string SupplierOrderId = "SupplierOrderId";

            public const string DistributorId = "DistributorId";
             
            public const string DetailSupplierOrder = "DetailSupplierOrder";

            public static class Details
            {
                public const string Price = "Price of product in details";

                public const string Quantity = "Quantity of product in details";

                public const string ProductId = "ProductId of product in details";

                public const string SupplierOrderId = "SupplierOrderId of product in details";
            }
        }

        public static class Promotion
        {
            public const int MinLimit = 1;

            public const string Module = "Promotion";

            public const string Id = "PromotionId";

            public const string Limit = "Limit";

            public const string Start = "Start";

            public const string End = "End";

            public const string Discount = "Discount";

            public const string PercentMax = "PercentMax";

            public const string Percent = "Percent";

            public const string DiscountMax = "DiscountMax";

            public const string Type = "Type";

            public const string Status = "Status";
        }

        public static class Coupon
        {
            public const int MinLimit = 1;

            public const string Module = "Coupon";

            public const string Id = "CouponId";

            public const string Limit = "Limit";

            public const string Start = "Start";

            public const string End = "End";

            public const string Discount = "Discount";

            public const string PercentMax = "PercentMax";

            public const string Percent = "Percent";

            public const string DiscountMax = "DiscountMax";

            public const string Type = "Type";

            public const string Status = "Status";
        }

        public static class Order
        {
            public const int MinQuantity = 1;

            public const string Module = "Order";

            public const string ProductId = "ProductId";

            public const string Quantity = "Quantity";
        }
    }
}
