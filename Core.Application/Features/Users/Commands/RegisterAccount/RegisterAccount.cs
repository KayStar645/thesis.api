using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Users.Commands.BaseUser;
using Core.Application.Features.Users.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;
using Microsoft.AspNetCore.Identity;

namespace Core.Application.Features.Users.Commands.RegisterAccount
{
    public record RegisterAccountCommand : IBaseUser, IRequest<Result<UserDto>>
    {
        public string? Name { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; } 

        public string? Address { get; set; }

        public string? Gender { get; set; }
    }

    public class RegisterAccountCommandHandler :
        CreateBaseCommandHandler<RegisterAccountValidator, RegisterAccountCommand, UserDto, User>
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterAccountCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, IPasswordHasher<User> pPasswordHasher,
            ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
            _passwordHasher = pPasswordHasher;
        }

        protected override async Task<User> Before(RegisterAccountCommand request)
        {
            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(user, request.Password);
            user.Type = User.UserType.User;

            return user;
        }

        protected override async Task After(RegisterAccountCommand request, User entity, UserDto dto)
        {
            await _mediator.Publish(new AfterRegisterAccountEvent(request));
        }
    }
}
