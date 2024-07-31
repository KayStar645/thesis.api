using Core.Application.Features.Users.Commands.BaseUser;
using Core.Application.Models.Common;
using static Core.Domain.Auth.User;

namespace Core.Application.Models
{
    public record UserDto : BaseDto, IBaseUser
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public UserType? Type { get; set; }

        public List<string>? Roles { get; set; }

        public int? StaffId { get; set; }

        public StaffDto? Staff { get; set; }

        public int? CustomerId { get; set; }

        public CustomerDto? Customer { get; set; }
    }
}
