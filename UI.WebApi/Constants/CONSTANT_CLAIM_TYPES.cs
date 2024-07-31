using Core.Domain.Auth;

namespace UI.WebApi.Constants
{
    public static class CONSTANT_CLAIM_TYPES
    {
        public const string Uid = "uid";

        public const string UserName = "userName";

        public const string Customer = "customer";

        public const string Type = "type";

        public const string Faculty = "faculty";

        public const string FacultyId = "facultyId";

        public const string Permission = "permission";
    }

    public static class CLAIMS_VALUES
    {
        public static string TYPE_ADMIN = User.UserType.Admin.ToString();

        public static string TYPE_SUPPER_ADMIN = User.UserType.SuperAdmin.ToString();

        public static string TYPE_USER = User.UserType.User.ToString();

    }
}
