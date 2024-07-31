namespace Core.Application.Features.Users.Commands.BaseUser
{
    public interface IBaseUser
    {
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
