using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Auth
{
	public class Permission : HardDeleteEntity
	{
        [Sieve(CanFilter = true, CanSort = true)]
        public string? Name { get; set; }

		public List<RolePermission>? RolePermissions { get; set; }

		public List<UserPermission>? UserPermissions { get; set; }
	}
}
