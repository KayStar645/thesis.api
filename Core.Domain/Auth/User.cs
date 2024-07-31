using Core.Domain.Common;
using Core.Domain.Entities;
using Sieve.Attributes;

namespace Core.Domain.Auth
{
    public class User : AuditableEntity
	{
        public enum UserType
        {
            Admin,
            SuperAdmin,
            User
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? UserName { get; set; }

		public string? Password { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Email { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? PhoneNumber { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public UserType Type { get; set; } = UserType.User;

        public List<UserRole>? UserRoles { get; set; }

        //Khoá ngoại
        [Sieve(CanFilter = true, CanSort = true)]
        public int? StaffId { get; set; }

		public virtual Staff? Staff { get; set; }


        [Sieve(CanFilter = true, CanSort = true)]
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
