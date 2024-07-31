using Core.Domain.Common;

namespace Core.Domain.Auth
{
	public class UserRole : HardDeleteEntity
	{
		public int? UserId { get; set; }

		public User? User { get; set; }

		public int? RoleId { get; set; }

		public Role? Role { get; set; }
	}
}
