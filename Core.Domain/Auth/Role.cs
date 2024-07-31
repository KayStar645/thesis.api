using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Auth
{
    public class Role : AuditableEntity
	{
        [Sieve(CanFilter = true, CanSort = true)]
        public string? Name { get; set; }

		public List<UserRole>? UserRoles { get; set; }

		public List<RolePermission>? RolePermissions { get; set; }
	}
}
