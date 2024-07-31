using Core.Application.Models.Common;

namespace Core.Application.Models
{
    public record RoleDto : BaseDto
    {
        public string? Name { get; set; }

        public List<string>? Permissions { get; set; }
    }
}
