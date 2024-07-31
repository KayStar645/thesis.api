using Core.Domain.Auth;
using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class StaffPositionHasRole : HardDeleteEntity
    {
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        public int? StaffPositionId { get; set; }

        public StaffPosition? StaffPosition { get; set; }
    }
}
