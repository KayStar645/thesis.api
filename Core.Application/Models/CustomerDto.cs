using Core.Application.Models.Common;

namespace Core.Application.Models
{
    public record CustomerDto : BaseDto
    {
        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public UserDto? User { get; set; }
    }
}
